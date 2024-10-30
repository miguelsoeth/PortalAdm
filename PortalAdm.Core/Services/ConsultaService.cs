using System.Net;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;
using PortalAdm.Core.Models;
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

        User u = await _userService.GetUserByEmailAsync(userMail);
        Product p = await _productService.GetProductById(productId);

        ConsultaOnlineMessage consulta = new ConsultaOnlineMessage(document, p, u);

        var response = await _portalConsultaService.ConsultaOnline(consulta);
        return response;
    }

    public async Task ConsultarLote(Guid productId, FileModel file, string? userMail)
    {
        User u = await _userService.GetUserByEmailAsync(userMail);
        Product p = await _productService.GetProductById(productId);
        List<string> documents = await FileUtil.GetDocumentsAsync(file.FileStream);
        decimal total = p.Price * documents.Count;

        ConsultaLoteMessage consulta = new ConsultaLoteMessage(documents, p, u, total);
        
        await _portalConsultaService.ConsultaLote(consulta);
    }
}