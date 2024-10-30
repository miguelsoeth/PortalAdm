using System.Net;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
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

    public async Task<ConsultaOnlineResult> ConsultarOnline(Guid productId, string document, string? userMail)
    {
        if(!DocumentUtil.CpfCnpj(document))
            throw new DefaultException("Documento inválido!", HttpStatusCode.BadRequest);
        
        if (userMail == null)
            throw new DefaultException("Erro ao adquirir informações do usuário atual!", HttpStatusCode.BadRequest);

        User u = await _userService.GetUserByEmailAsync(userMail);
        Product p = await _productService.GetProductById(productId);

        ConsultaOnlineMessage consulta = new ConsultaOnlineMessage(document, p, u);

        var response = await _portalConsultaService.ConsultaOnline(consulta);
        return response;
    }
}