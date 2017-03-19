/*using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_api_anais.Models
{
    public class RecipeRepository : IRecipeRepository
    {
        private static ConcurrentDictionary<int, Recipe> _recipes = new ConcurrentDictionary<int, Recipe>();

        public RecipeRepository()
        {
            Add(new Recipe { Name = "recipe1" });
        }

        public void Add(Recipe recipe)
        {
            _recipes[recipe.Id] = recipe;
        }

        public Recipe Find(int id)
        {
            Recipe recipe;
            _recipes.TryGetValue(id, out recipe);
            return recipe;
            
        }

        public IEnumerable<Recipe> GetAll()
        {
            return _recipes.Values;
        }

        public Recipe Remove(int id)
        {
            Recipe recipe;
            _recipes.TryRemove(id, out recipe);
            return recipe;
        }

        public void Update(Recipe recipe)
        {
            _recipes[recipe.Id] = recipe;
        }
    }
}
*/