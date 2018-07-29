using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using StudentSystem.Data.Identity;
using System.Web.Mvc.Expressions;
using StudentSystem.Clients.Mvc.ViewModels.Account;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService)); ;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authenticationService.LogIn(model.Email, model.Password, model.RememberMe);

            if (result == SignInStatus.Success)
            {
                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            // TODO error
            //ModelState.AddModelError(string.Empty, Resources.Resources.IncorrentEmailOrPasswordMessage);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return this.RedirectToAction<AccountController>(x => x.Login());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            RegisterViewModel model = new RegisterViewModel()
            {
                
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authenticationService.CreateAccountAsync(
                model.Email,
                model.Password);

            if (result.Succeeded)
            {
                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            AddErrors(result);
          
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}