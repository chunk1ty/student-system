using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using System.Web.Security;
using StudentSystem.Clients.Mvc.ViewModels.Account;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

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
                FormsAuthentication.SetAuthCookie(model.Email, true);

                return this.RedirectToAction<CourseController>(x => x.AvailableCourses());
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }
    }
}