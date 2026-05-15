using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity.Providers;

public class JwtProvider(AppSetting appSetting)
{
    public virtual string GenerateToken(User user)
    {
        var privateKey = File.ReadAllText(appSetting.Jwt.PrivateKeyPath);
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);

        var key = new RsaSecurityKey(rsa);
        var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: appSetting.Jwt.Issuer,
            audience: appSetting.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(appSetting.Jwt.ExpiryInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
