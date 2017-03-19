using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_api_anais.Models
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/user/login";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(15);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
