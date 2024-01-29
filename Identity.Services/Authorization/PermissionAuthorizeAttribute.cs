using Identity.Services.Authorization.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;


namespace Identity.Services.Authorization;

//B way
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    internal const string PolicyPerfix = "PERMISSION: ";
    public PermissionAuthorizeAttribute(params string[] permissions)
    {
        Policy = $"{PolicyPerfix}{string.Join(",", permissions)}";
    }
}


public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }
    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if(policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPerfix,StringComparison.OrdinalIgnoreCase))
        {
            return base.GetPolicyAsync(policyName);
        }

        var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPerfix.Length).Split(",");
        var policy = new AuthorizationPolicyBuilder()
            .RequireClaim(AuthorizePermissionConsts.Permission, permissionNames)
            .Build();
        return Task.FromResult(policy);
    }
}