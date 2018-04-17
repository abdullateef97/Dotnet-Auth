using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestIdentity.Models;

namespace TestIdentity.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<TestIdentityUser> _userManager;

        public HomeController(UserManager<TestIdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(registerModel.UserName);

                if (user == null)
                {
                    user = new TestIdentityUser()
                    {
                    
                        Id = Guid.NewGuid().ToString(),
                        UserName = registerModel.UserName
                    };

                    var result = await _userManager.CreateAsync(user, registerModel.Password);
                }

                return View("Success");
            }
            return View();
        }
        
        
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user != null  && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var identity = new ClaimsIdentity("cookies");
                    
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));
                   Console.WriteLine(1);
                   return View("Index");
                }
                ModelState.AddModelError("", "Invalid UserName and Password");
            }
            

            return View();

        }
        
    }
}