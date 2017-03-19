using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Models
{
    public class AuthTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public AuthTokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetToken(string val)
        {
            _session.SetString("access_token", val);
        }

        public string GetToken()
        {
            return _session.GetString("access_token");
        }

        public void Remove()
        {
            _session.Remove("access_token");
            _session.Remove("isAdmin");
        }

        
        public void SetAdmin(int val)
        {
            _session.SetInt32("isAdmin", val);
        }

        public Boolean IsAdmin()
        {
            if (_session.GetInt32("isAdmin") == 1)
                return true;
            return false;
        }
    }
}
