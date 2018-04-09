using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
    }
}