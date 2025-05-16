using FrontEnd.ApiModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        ISecurityHelper securityHelper;

        public LoginController(ISecurityHelper securityHelper)
        {
                this.securityHelper = securityHelper;
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            UserViewModel user = new UserViewModel();
            user.ReturnUrl = ReturnUrl;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user) {
            try
            {
                if (ModelState.IsValid) 
                {
                    var loging = securityHelper.Login(user);

                    TokenAPI tokenAPI = loging.Token;
                    var EsValido = false;

                    if (tokenAPI != null) {
                        HttpContext.Session.SetString("token", tokenAPI.Token);
                        EsValido = true;
                    }

                    if (!EsValido)
                    {
                        ViewBag.Message = "Invalid Credentials";
                        return View(user);
                    }


                    var claims = new List<Claim>() {
                                     new Claim(ClaimTypes.NameIdentifier, loging.Username as string),
                                         new Claim(ClaimTypes.Name, loging.Username as string)
                    };

                    foreach (var item in loging.Roles)
                    {
                        claims.Add(
                              new Claim(ClaimTypes.Role, item as string)

                            );
                    }


                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = user.RememberLogin
                    });


                    return LocalRedirect(user.ReturnUrl);
                }
                return View(user);

            }
            catch (Exception)
            {

                throw;
            }
        
        }
    }
}
