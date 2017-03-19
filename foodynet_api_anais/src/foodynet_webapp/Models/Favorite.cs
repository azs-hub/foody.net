//using foodynet_api_anais.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int UserId { get; set; }

        public virtual Recipe Recipe { get; set; }
        //public virtual User User { get; set; }
    }
}