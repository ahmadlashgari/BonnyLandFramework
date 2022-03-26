using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using System.Threading.Tasks;

namespace BL.Framework.IdentityServer
{
    public class IdProfileService : IProfileService
    {
        public IdProfileService() { }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            //var requestClaims = context.Subject.FindAll("api.okcard.request");
            //var customerClaims = context.Subject.FindAll("api.okcard.customer");

            context.IssuedClaims.AddRange(roleClaims);
            //context.IssuedClaims.AddRange(requestClaims);
            //context.IssuedClaims.AddRange(customerClaims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
