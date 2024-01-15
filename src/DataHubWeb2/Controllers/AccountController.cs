using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DataHubWeb2.Model;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Web;

namespace DataHubWeb2.Controllers {
    public class AccountController : BaseController {
        // GET: /Account/SignIn
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInViewModel() {UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name, RememberMe = true });
        }

        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInViewModel model, string returnUrl) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            //System.Web.Helpers.AntiForgery.Validate();

            var userIdentity = new ClaimsIdentity("Custom");

            // DXCOMMENT: You Authentication logic
            if (AuthHelper.SignIn(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                //var authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, "");
                var authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMonths(1), true, "");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);           

                if (returnUrl == null)
                    return RedirectToAction("Index", "Home");
                else
                    return Redirect(returnUrl);
            }
            else
            {
                SetErrorText("Invalid login attempt.");
                ModelState.AddModelError("", ViewBag.GeneralError);
                return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if(ModelState.IsValid) {
                // DXCOMMENT: Your Registration logic 
            }

            return View(model);
        }

        // POST: /Account/SignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut() {
            AuthHelper.SignOut();

            // DXCOMMENT: Your Signing out logic
            FormsAuthentication.SignOut();


            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserMenuItemPartial() {
            return PartialView("UserMenuItemPartial", AuthHelper.GetLoggedInUserInfo());
        }
    }
}