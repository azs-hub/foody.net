using foodynet_api_anais.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace foodynet_api_anais.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly MyDbContext _context;
        private static User _user = new User();
        private static IHttpContextAccessor _httpContextAccessor;
        
        public UsersController(MyDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET api/users
        [Authorize(Roles = "Admin")]
        [HttpGet("Getall")]
        public IActionResult GetAll()
        {
            var user = _context.Users.ToList();
            if (user != null)
                return Ok(user);
            return NotFound();
        }

        // GET Current user for restricted route
        [HttpGet]
        public Boolean GetCurrent()
        {
            var res = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (res != null)
            {
                _user = _context.Users.FirstOrDefault(r => r.Email == res);
                return true;
            }
            return false;
        }

        // POST api/user/signin -- inscription
        [HttpPost]
        [Route("Signup")]
        public IActionResult Signup([FromBody]User user)
        {
            if (user.Password == null || user.Email == null)
                return NotFound();

            var _user = _context.Users.FirstOrDefault(r => r.Email == user.Email);

            if (_user != null)
                return new BadRequestObjectResult("The user already exist");

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                user.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();   
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // GET api/users/favorites
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Favorites")]
        public IActionResult GetFavorites()
        {
            if (!GetCurrent())
                return NotFound();

            if (_user == null)
                return NotFound();

            var favorites = _context.Favorites
                            .Include(favorite => favorite.Recipe)
                            .Where(favorite => favorite.UserId == _user.Id)
                            .ToList();

            if (favorites != null)
                return Ok(favorites);
            return NotFound();
        }

        // POST api/users/favorite
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [Route("Favorite")]
        public IActionResult AddFavorite([FromBody]Favorite favorite)
        {
            if (!GetCurrent() && favorite != null && favorite.RecipeId != null)
                return NotFound();

            var isset = _context.Favorites.Where(fav => fav.UserId == _user.Id && fav.RecipeId == favorite.RecipeId).SingleOrDefault();
            
            if (isset != null)
                return NotFound();
            favorite.UserId = _user.Id;
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return Ok(favorite);
        }

        // DELETE api/users/favorite
        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        [Route("Favorite/{id}")]
        public IActionResult RemoveFavroite(int id)
        {
            if (!GetCurrent())
                return NotFound();

            var isset = _context.Favorites.Where(fav => fav.UserId == _user.Id && fav.Id == id).SingleOrDefault();

            if (isset == null)
                return NotFound();

            _context.Favorites.Remove(isset);
            _context.SaveChanges();
            return Ok();
        }

        // PUT api/users
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody]User user)
        {
            var newUser = _context.Users.FirstOrDefault(r => r.Id == user.Id);
            newUser.Admin = user.Admin;

            _context.Update(newUser);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE api/user/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(r => r.Id == id);
            var favorites = _context.Favorites.Where(r => r.UserId == id).ToList();

            if (user != null)
            {
                _context.Remove(user);
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
