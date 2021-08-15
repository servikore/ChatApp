using Application.DomainServices;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index(string ReturnUrl = "/")
        {
            var loginModel = new LoginModel();
            loginModel.ReturnUrl = ReturnUrl;
            return View(loginModel);
        }
        
        public IActionResult Register()
        {
            var registrationModel = new RegisterModel();            
            return View(registrationModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userService.RegisterAsync(new User
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        NickName = registerModel.NickName
                    });

                    await SignIn(user);
                    return RedirectToAction("index", "home");
                }
                catch (InvalidUserException) 
                {
                    ViewBag.Message = "Information provided is not valid.";
                    return View(registerModel);
                }                
            }

            return View(registerModel);            
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    var user = await userService.AuthenticateAsync(loginModel.Email, loginModel.Password);
                    
                    await SignIn(user, loginModel.Remember);

                    return RedirectToAction("index", "home");
                }
                catch (AuthenticationException)
                {
                    ViewBag.Message = "Invalid Username or Password";
                    return View("index", loginModel);
                }               
            }
            return View("index", loginModel);
        }

        private async Task SignIn(User user, bool rememberMe = false)
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("nickname", user.NickName)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = rememberMe
            });            
        }

        public async Task<IActionResult> LogOut()
        {             
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);         
            return LocalRedirect("/");
        }
    }
}
