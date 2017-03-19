using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_api_anais.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }

        public int Admin { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
