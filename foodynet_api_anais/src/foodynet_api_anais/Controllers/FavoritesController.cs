using foodynet_api_anais.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace foodynet_api_anais.Controllers
{
    [Route("api/[controller]")]
    public class FavoritesController : Controller
    {
        private readonly MyDbContext _context;
        private static Favorite favorite = new Favorite();

        public FavoritesController(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
        }

        // GET api/favorites
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null)
                return NotFound();

            var favorites = _context.Favorites
                            .Where(r => r.UserId == id)
                            .Include(favorite => favorite.Recipe)
                            .ToList();

            if (favorites != null)
                return Ok(favorites);
            return NotFound();
        }

        // POST api/favorite
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public void Post([FromBody]Favorite value)
        {
            _context.Favorites.Add(value);
            _context.SaveChanges();
        }

        // DELETE api/recipes/5
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
