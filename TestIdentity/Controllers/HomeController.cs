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
        private readonly IUserClaimsPrincipalFactory<TestIdentityUser> _claimsPrincipalFactory;

        public HomeController(UserManager<TestIdentityUser> userManager,
            IUserClaimsPrincipalFactory<TestIdentityUser> claimsPrincipalFactory)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
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
                        UserName = registerModel.UserName,
                        Email = registerModel.Email
                    };

                    var result = await _userManager.CreateAsync(user, registerModel.Password);
                    
                    //confirm Email Address
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationEmailLink = Url.Action("ConfirmationEmail", "Home",
                            new {token = token, email = user.Email}, Request.Scheme);
                        System.IO.File.WriteAllText("confirmationEmailLink.txt",confirmationEmailLink);
                    }
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
//                    var identity = new ClaimsIdentity("Identity.Application");
//                    
//                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
//                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("","Email is not confirmed");
                        return View();
                    }
                    var principal = await _claimsPrincipalFactory.CreateAsync(user);

                    await HttpContext.SignInAsync("Identity.Application",principal);
                   Console.WriteLine(1);
                   return View("Index");
                }
                ModelState.AddModelError("", "Invalid UserName and Password");
            }
            

            return View();

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> forgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.EmailAddress);
                Console.WriteLine("use r is"+user);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var tokenUrl = Url.Action("ResetPassword", "Home",
                        new {token = token, email = user.Email}, Request.Scheme);
                    Console.WriteLine("token is"+tokenUrl);
                    System.IO.File.WriteAllText("resetpassword.txt", tokenUrl);
                }

                return View("Success");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordModel {Token = token, Email = email});
        }


        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> resetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token,
                        resetPasswordModel.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("",error.Description);
                        }
                        return View();
                    }
                    return View("Login");
                }
                ModelState.AddModelError("","Invalid Request");
            }
            return View();
        }

        public async Task<IActionResult> ConfirmationEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return View("Success");
                }
            }
            return View("Error");
        }
    }
}