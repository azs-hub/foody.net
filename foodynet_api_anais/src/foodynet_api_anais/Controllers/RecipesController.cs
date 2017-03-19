using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using foodynet_api_anais.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace foodynet_api_anais.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly MyDbContext _context;
        private static Recipe recipe = new Recipe();
        

        public RecipesController(MyDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IActionResult GetAll([FromQuery]string p)
        {
            int page = Convert.ToInt32(p);
            page = (page >= 1) ? page : 1;
            var _recipes = _context.Recipes.Skip((page - 1) * 5).Take(5).ToList();
            var _total = (int)Math.Ceiling( _context.Recipes.Count() / (double)5 );

            if (recipe != null)
                return Ok( new { recipes= _recipes, total = _total, current = page } );
            return NotFound();
        }

        // GET api/recipes/5
        [HttpGet("{id}")]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);

            if (recipe != null)
                return Ok(recipe);
            return NotFound();

        }

        // POST api/recipes
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody]Recipe value)
        {
            _context.Recipes.Add(value);
            _context.SaveChanges();
            return Ok(value);
        }

        // PUT api/recipes/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody]Recipe value)
        {
            _context.Update(value);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE api/recipes/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            var favorites = _context.Favorites.Where(r => r.RecipeId == id).ToList();

            if (recipe != null)
            {
                _context.Remove(recipe);
                _context.SaveChanges();
                return Ok();
            }
            if (favorites != null)
            {
                _context.Remove(favorites);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
