using foodynet_webapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodynet_webapp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _user;
        
        public UserController(UserService user)
        {
            _user = user;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signup")]
        public IActionResult SignupView()
        {
            return View();
        }

        [HttpPost("signup")]
        public IActionResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                if(_user.Signup(user))
                    return RedirectToAction("SigninView");
            }
            return RedirectToAction("SignupView");
        }

        [HttpGet("signin")]
        public IActionResult SigninView(User user)
        {
            return View();
        }

        [HttpPost("signin")]
        public IActionResult Signin(User user)
        {
            if (ModelState.IsValid)
            {
                if (_user.Signin(user))
                    return RedirectToAction("Index", "Recipes");
            }
            return RedirectToAction("SigninView");
        }


        [HttpGet("favorites")]
        public IActionResult Favorites()
        {
            var data = _user.GetFavorites();
            if (data != null)
                return View(_user.GetFavorites());
            return RedirectToAction("Error", "Home");
        }

        [HttpPost("favorite/add")]
        public IActionResult AddFavorite(Favorite favorite)
        {
            _user.AddFavorite(favorite);
            return RedirectToAction("RecipeDetail", "Recipes", new {id = favorite.RecipeId });
        }

        [HttpPost("favorite/remove")]
        public IActionResult RemoveFavorite(Favorite favorite)
        {
            _user.RemoveFavorite(favorite.Id);
            return RedirectToAction("RecipeDetail", "Recipes", new { id = favorite.RecipeId });
        }

        [HttpGet("admin/users")]
        public IActionResult Manage()
        {
            if (_user.IsAuth() && _user.IsAdmin())
                return View(_user.GetAll());
            return RedirectToAction("Error", "Home");
        }

        [HttpPost("admin/users")]
        public IActionResult Update(User user)
        {
            if (_user.Update(user))
                return RedirectToAction("Manage");
            return RedirectToAction("Manage");
        }

        [HttpGet("admin/users/remove/{id}")]
        public IActionResult Remove(int id)
        {
            if (_user.Remove(id))
                return RedirectToAction("Manage");
            return RedirectToAction("Manage");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _user.Logout();
            return RedirectToAction("Index", "Recipes");
        }
    }
}
