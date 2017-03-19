using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_api_anais.Models
{
    public class Recipe
    {
        //internal object RecipeId;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Ingredient { get; set; }
        public string cooking { get; set; }
        public int Time { get; set; }

        //public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
