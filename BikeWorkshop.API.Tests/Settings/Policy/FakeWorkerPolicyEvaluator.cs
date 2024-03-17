using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BikeWorkshop.API.Tests.Settings.Policy;
public class FakeWorkerPolicyEvaluator : IPolicyEvaluator
{
	public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
	{
		var claims = new ClaimsPrincipal();

		claims.AddIdentity(new ClaimsIdentity(
			new[]
			{
					new Claim(ClaimTypes.NameIdentifier,Constants.EmployeeId.ToString()),
					new Claim(ClaimTypes.Role,"Worker"),
			}));
		context.User = claims;
		var ticket = new AuthenticationTicket(claims, "Test");
		var result = AuthenticateResult.Success(ticket);
		return Task.FromResult(result);
	}

	public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
	{
		List<IAuthorizationRequirement>? requirements = policy.Requirements.ToList();
		List<string>? roleList = new List<string>();

		foreach (var requirement in requirements)
		{
			var roles = requirement.ToString();
			if (roles != null && roles.Contains("Worker")) { roleList.Add("Worker"); }
			if (roles != null && roles.Contains("Manager")) { roleList.Add("Manager"); }
		}

		if (!roleList.Any())
		{
			var success = PolicyAuthorizationResult.Success();
			return Task.FromResult(success);
		}

		var claims = context
			.User
			.Claims?
			.Where(x => x.Type == ClaimTypes.Role)
			.Select(x => x.Value)
			.ToList();

		var isAnyClaimInRoleList = claims?.Intersect(roleList).Any() ?? false;

		var result = isAnyClaimInRoleList
			? PolicyAuthorizationResult.Success()
			: PolicyAuthorizationResult.Forbid();

		return Task.FromResult(result);
	}
}
