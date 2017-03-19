using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using foodynet_webapp.Models;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace foodynet_webapp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeService _recipe;
        private readonly UserService _user;
        private readonly HttpContext _context;

        public RecipesController(RecipeService recipe,
            UserService user,
            IHttpContextAccessor httpContextAccessor)
        {
            _recipe = recipe;
            _user = user;
            _context = httpContextAccessor.HttpContext;
        }

        [Route("")]
        [Route("recipes")]
        public IActionResult Index()
        {
            PaginatedList data = _recipe.GetAll(_context.Request.Query["p"]);
            return View(data);
        }

        [Route("recipe/{id}")]
        public IActionResult RecipeDetail(int id)
        {
            Recipe recipe = _recipe.GetRecipeById(id);
            IEnumerable<Favorite> favorites = _user.GetFavorites();

            if (favorites != null)
            {
                var favorite = favorites.Where(f => f.RecipeId == id).FirstOrDefault();
                if (favorite != null)
                    ViewBag.favorite = favorite;
            }

            if (recipe != null)
                return View(recipe);
            return RedirectToAction("Error", "Home");
        }

        [Route("addrecipe")]
        public IActionResult CreateRecipeView()
        {
            if(_user.IsAuth() && _user.IsAdmin())
                return View();

            return RedirectToAction("Error", "Home");

        }

        public IActionResult CreateRecipe(Recipe recipe)
        {
            Recipe newRecipe = _recipe.CreateRecipe(recipe);
            if (newRecipe != null)
                return RedirectToAction("RecipeDetail", new { id = newRecipe.Id });
            return RedirectToAction("CreateRecipeView");

        }

        public IActionResult UpdateRecipe(Recipe recipe)
        {
            _recipe.UpdateRecipe(recipe);
            return RedirectToAction("RecipeDetail", new { id = recipe.Id });
            
        }

        public IActionResult Remove(Recipe recipe)
        {
            _recipe.RemoveRecipe(recipe);
            return RedirectToAction("Index");

        }
    }
}
