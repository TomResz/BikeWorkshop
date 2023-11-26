using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeWorkshop.Infrastructure.Services;

public class JwtService : IJwtService
{
	private readonly AuthenticationSettings _authenticationSetting;

	public JwtService(AuthenticationSettings authenticationSetting)
	{
		_authenticationSetting = authenticationSetting;
	}

	public string GetToken(Employee employee)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),
			new Claim(ClaimTypes.Name,$"{employee.FirstName} {employee.LastName}"),
			new Claim(ClaimTypes.Email,employee.Email),
			new Claim(ClaimTypes.Role,employee.Role.Name),
		};
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSetting.JwtKey));
		var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddDays(_authenticationSetting.JwtExpireDays);
		var token = new JwtSecurityToken(
				issuer: _authenticationSetting.JwtIssuer,
				audience: _authenticationSetting.JwtIssuer,
				claims: claims,
				expires: expires,
				signingCredentials: cred);
		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}
}
