using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BikeWorkshop.Infrastructure.Services;
internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly AuthenticationSettings _authenticationSetting;

    public RefreshTokenService(AuthenticationSettings authenticationSetting)
    {
        _authenticationSetting = authenticationSetting;
    }

    public RefreshToken GenerateRefreshToken(Guid EmployeeId, DateTime date)
    {
        var expirationTime = date.Add(TimeSpan.FromDays(30));

        byte[] randomHash = new byte[64];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomHash);
        string hashString = Convert.ToBase64String(randomHash);

        return new RefreshToken
        {
            EmployeeId = EmployeeId,
            ExpirationTimeUtc = expirationTime,
            Id = Guid.NewGuid(),
            Token = hashString,
        };
    }

    public Guid? GetUserIdFromExpiredToken(string oldAccessToken)
    {
        var claims = PrincipalsFromExpiredToken(oldAccessToken);
        var idValue = claims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(idValue, out var id))
        {
            return id;
        }

        return null;
    }

    public ClaimsPrincipal? PrincipalsFromExpiredToken(string oldAccessToken)
    {
        var tokenParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSetting.JwtKey)),
            ValidIssuer = _authenticationSetting.JwtIssuer,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateLifetime = false,
        };

        var handler = new JwtSecurityTokenHandler();

        return handler.ValidateToken(oldAccessToken, tokenParameters, out _);
    }
}
