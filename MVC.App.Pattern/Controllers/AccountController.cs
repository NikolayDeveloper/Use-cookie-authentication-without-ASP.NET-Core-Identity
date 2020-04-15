using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC.App.Pattern.Models;

namespace MVC.App.Pattern.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        private List<User> _users;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [AllowAnonymous, HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel() {ReturnUrl=returnUrl });
        }
        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
            bool isAuthenticated;
            if(!ModelState.IsValid)
            {
                return View(loginModel);
            }
            isAuthenticated= FindCredentialUser(loginModel);
            if(!isAuthenticated)
            {
                ModelState.AddModelError("", "Invalid login or password");
                return View(loginModel);
            }
            if (Url.IsLocalUrl(loginModel.ReturnUrl))
            {
                return Redirect(loginModel.ReturnUrl);
            }
            return RedirectToAction("Index","Home");
            
        }
        /// <summary>
        /// Find Credential user in apsettings.json file
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        private bool FindCredentialUser(LoginModel loginModel)
        {
            _users = _configuration.GetSection("ApplicationUsers").Get<List<User>>();
            foreach (User user in _users)
            {
                if(user.Login==loginModel.Login &&
                    user.Password==loginModel.Password)
                {
                    SetUserCookies(user).GetAwaiter().GetResult();
                    return true;
                }
            }
            return false;
        }
        private async Task  SetUserCookies(User user)
        {
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Login)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
    }
}