﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using StudentSystem.Authentication;
using StudentSystem.Clients.Mvc.Infrastructure;
using StudentSystem.Clients.Mvc.ViewModels.Account;
using StudentSystem.Domain.Services.Contracts;

namespace StudentSystem.Clients.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _accountService;
        private readonly FormsAuthenticationWrapper _formsAuthenticationWrapper;

        public AccountController(
            IAuthenticationService accountService, 
            FormsAuthenticationWrapper formsAuthenticationWrapper)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _formsAuthenticationWrapper = formsAuthenticationWrapper ?? throw new ArgumentNullException(nameof(formsAuthenticationWrapper));
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction<CourseController>(x => x.Index());
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var operation =  await _accountService.LogInAsync(model.Email, model.Password);
            if (operation.IsSuccessful)
            {
                _formsAuthenticationWrapper.SetAuthCookie(model.Email, model.RememberMe);

                return this.RedirectToAction<CourseController>(x => x.Index());
            }

            ModelState.AddModelError(string.Empty, operation.ErrorMessage);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _formsAuthenticationWrapper.SignOut();

            return this.RedirectToAction<AccountController>(x => x.Login());
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var operation =  await _accountService.RegisterAsync(model.Email, model.Password);
            if (operation.IsSuccessful)
            {
                _formsAuthenticationWrapper.SetAuthCookie(model.Email, true);

                return this.RedirectToAction<CourseController>(x => x.Index());
            }

            ModelState.AddModelError(string.Empty, operation.ErrorMessage);
            return View(model);
        }
    }
}