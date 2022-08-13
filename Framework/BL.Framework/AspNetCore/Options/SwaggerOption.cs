using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Framework.AspNetCore.Options
{
    public class SwaggerOption
    {
        public string SecurityDefinitionName { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string DocVersion { get; set; }
        public string DocTitle { get; set; }
        public string DocDescription { get; set; }
        public string DocTermsOfService { get; set; }
        public string DocContactName { get; set; }
        public string DocContactEmail { get; set; }
        public string RouteTemplate { get; set; }
        public string EndpointUrl { get; set; }
        public string EndpointTitle { get; set; }
        public string RoutePrefix { get; set; }
        public string OAuthClientId { get; set; }
        public string OAuthClientSecret { get; set; }
        public Dictionary<string, string> OAuthScopes { get; set; }
    }
}
