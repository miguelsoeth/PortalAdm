using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Core.Services;

public class TokenService : ITokenService
{
    
    public async Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AmbienteUtil.GetValue("JWT_KEY")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: AmbienteUtil.GetValue("JWT_ISSUER"),
            audience: AmbienteUtil.GetValue("JWT_AUDIENCE"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}