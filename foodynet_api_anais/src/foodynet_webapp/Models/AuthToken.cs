using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Models
{
    public class AuthToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }

        public User user { get; set; }
    }
}
