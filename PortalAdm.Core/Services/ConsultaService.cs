using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Core.Services;

public class ConsultaService : IConsultaService
{
    private readonly IUserService _userService;
    private readonly IProductService _productService;
    private readonly IPortalConsultaService _portalConsultaService;

    public ConsultaService(IUserService userService, IProductService productService, IPortalConsultaService portalConsultaService)
    {
        _userService = userService;
        _productService = productService;
        _portalConsultaService = portalConsultaService;
    }

    public async Task<AuthResponse> ConsultarOnline(ConsultaOnlineRequest consultaRequest, string? userMail)
    {
        if(!DocumentUtil.CpfCnpj(consultaRequest.Document))
            return new AuthResponse(false, String.Empty, "Documento inválido!");
        
        if (userMail == null)
            return new AuthResponse(false, String.Empty, "Erro ao adquirir informações do usuário atual!");

        User? u = await _userService.GetUserByEmailAsync(userMail);
        
        if (u == null)
            return new AuthResponse(false, String.Empty, "Usuário não encontrado!");

        Product? p = await _productService.GetProductById(consultaRequest.ProductId);
        
        if (p == null)
            return new AuthResponse(false, String.Empty, "Produto não encontrado!");

        ConsultaOnlineMessage consulta = new ConsultaOnlineMessage(consultaRequest.Document, p, u);

        var response = await _portalConsultaService.ConsultaOnline(consulta);
        
        return new AuthResponse(true, String.Empty, "Consulta gerada com sucesso!");
    }
}