using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using System.Web.Security;
using StudentSystem.Clients.Mvc.Infrastructure;
using StudentSystem.Clients.Mvc.ViewModels.Account;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly FormsAuthenticationWrapper _formsAuthenticationWrapper;

        public AccountController(
            IAccountService accountService, 
            FormsAuthenticationWrapper formsAuthenticationWrapper)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _formsAuthenticationWrapper = formsAuthenticationWrapper ?? throw new ArgumentNullException(nameof(formsAuthenticationWrapper));
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

            var result =  await _accountService.LogInAsync(model.Email, model.Password);

            if (result.IsSuccessful)
            {
                _formsAuthenticationWrapper.SetAuthCookie(model.Email, model.RememberMe);

                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _formsAuthenticationWrapper.SignOut();

            return this.RedirectToAction<AccountController>(x => x.Login());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
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

            var result =  await _accountService.RegisterAsync(model.Email, model.Password);

            if (result.IsSuccessful)
            {
                _formsAuthenticationWrapper.SetAuthCookie(model.Email, true);

                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }
    }
}