using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebApiClaimsAuthorization.Models;

namespace WebApiClaimsAuthorization.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (model.UserName != model.Password) return View();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Email, "user@email.com"),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim("Data", "Read"),                    
            };

            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            var authenticationManager = Request.GetOwinContext().Authentication;

            var authProperties = new AuthenticationProperties { IsPersistent = true };

            authenticationManager.SignIn(authProperties, id);
            
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var authenticationManager = Request.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return Redirect("/");
        }
    }
}