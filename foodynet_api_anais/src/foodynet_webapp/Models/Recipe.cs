using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ingredient { get; set; }
        public string cooking { get; set; }
        public int Time { get; set; }
    }
}
