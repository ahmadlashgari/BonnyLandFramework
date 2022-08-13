using System.Collections.Generic;

namespace BL.Framework.AspNetCore.Options
{
    public class AuthorizationPolicyOption
    {
        public string Name { get; set; }
        public List<string> RequireRole { get; set; }
        public List<AuthorizationPolicyClaimOption> RequireClaim { get; set; }
        public List<string> RequireScope { get; set; }
        public bool RequireAuthenticatedUser { get; set; }
    }

    public class AuthorizationPolicyClaimOption
    {
        public string Type { get; set; }
        public List<string> AllowedValues { get; set; }
    }
}
