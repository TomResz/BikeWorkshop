using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BikeWorkshop.API.Tests.Settings.Policy;
public class FakeManagerPolicyEvaluator : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var claimsPrincipal = new ClaimsPrincipal();

		claimsPrincipal.AddIdentity(new ClaimsIdentity(
    new[]
    	{
            new Claim(ClaimTypes.NameIdentifier,Constants.ManagerId.ToString()),
            new Claim(ClaimTypes.Role,"Manager"),
    	}));
		context.User = claimsPrincipal;
		var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
    {
        List<IAuthorizationRequirement>? requirements = policy.Requirements.ToList();
        List<string>? roleList = new List<string>();

        foreach(var requirement in requirements)
        {
            var roles = requirement.ToString();
            if (roles !=null && roles.Contains("Worker")) { roleList.Add("Worker"); }
            if (roles !=null && roles.Contains("Manager")) { roleList.Add("Manager"); }
		}

        if(!roleList.Any())
        {
            var success = PolicyAuthorizationResult.Success();
            return Task.FromResult(success);    
        }

        var claims = context
            .User
            .Claims?
            .Where(x=> x.Type == ClaimTypes.Role)
            .Select(x=>x.Value)
            .ToList();

        var isAnyClaimInRoleList = claims?.Intersect(roleList).Any() ?? false;

        var result = isAnyClaimInRoleList 
            ? PolicyAuthorizationResult.Success() 
            : PolicyAuthorizationResult.Forbid();

        return Task.FromResult(result);
    }
}
