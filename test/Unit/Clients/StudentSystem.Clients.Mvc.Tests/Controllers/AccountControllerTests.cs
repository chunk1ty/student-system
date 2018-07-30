using System;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

using StudentSystem.Clients.Mvc.Controllers;
using StudentSystem.Clients.Mvc.Infrastructure;
using StudentSystem.Clients.Mvc.ViewModels.Account;
using StudentSystem.Common;
using StudentSystem.Data.Services;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Clients.Mvc.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAccountService> _mockedAuthenticationService;
        private Mock<FormsAuthenticationWrapper> _mockedFormsAuthenticationWrapper;

        [SetUp]
        public void SetUp()
        {
            _mockedAuthenticationService = new Mock<IAccountService>();
            _mockedFormsAuthenticationWrapper = new Mock<FormsAuthenticationWrapper>();
        }

        [Test]
        public void Constructor_WithNullAccountService_ShouldThrowsArgumentNullException()
        {
            var ex = Assert.Catch<ArgumentNullException>(() => new AccountController(null, new FormsAuthenticationWrapper()));

            StringAssert.Contains("accountService", ex.Message);
        }

        [Test]
        public void Constructor_WithNullIFormsAuthenticationWrapper_ShouldThrowsArgumentNullException()
        {
            var ex = Assert.Catch<ArgumentNullException>(() => new AccountController(_mockedAuthenticationService.Object, null));

            StringAssert.Contains("formsAuthenticationWrapper", ex.Message);
        }

        [Test]
        public void LoginOnGetRequest_WithAuthenticateUser_ShouldRedirectToAvailableCourses()
        {
            // Arrange
            var mockContext = new Mock<ControllerContext>();

            mockContext.Setup(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);

            var accountController = new AccountController(_mockedAuthenticationService.Object, _mockedFormsAuthenticationWrapper.Object)
            {
                ControllerContext = mockContext.Object
            };

            // Act & Assert
            accountController.WithCallTo(c => c.Login())
                             .ShouldRedirectTo<CourseController>(x => x.AvailableCourses());
        }

        [Test]
        public void LoginOnGetRequest_WithNotAuthenticateUser_ShouldRenderLoginView()
        {
            // Arrange
            var mockContext = new Mock<ControllerContext>();
            mockContext.Setup(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            var accountController = new AccountController(_mockedAuthenticationService.Object, _mockedFormsAuthenticationWrapper.Object)
            {
                ControllerContext = mockContext.Object
            };

            // Act & Assert
            accountController.WithCallTo(c => c.Login())
                             .ShouldRenderDefaultView();
        }

        [Test]
        public void LoginOnPostRequest_WithInvalidModelState_ShouldRenderLoginView()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            accountController.ModelState.AddModelError("errorKey", "error");

            var model = new LoginViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Login(model))
                             .ShouldRenderDefaultView()
                             .WithModel(model)
                             .AndModelError("errorKey")
                             .ThatEquals("error");
        }

        [Test]
        public void LoginOnPostRequest_WithValidModelStateAndSuccessResult_ShouldRedirectToAvailableCoursesView()
        {
            // Arrange
            _mockedAuthenticationService.Setup(x => x.LogInAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(new SuccessStatus<string>(string.Empty));

            _mockedFormsAuthenticationWrapper.Setup(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()));

            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            var model = new LoginViewModel
            {
                Email = "Email",
                RememberMe = true
            };

            // Act & Assert
            accountController.WithCallTo(c => c.Login(model))
                             .ShouldRedirectTo<CourseController>(x => x.AvailableCourses());

            _mockedFormsAuthenticationWrapper.Verify(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void LoginOnPostRequest_WithValidModelStateAndNotSuccessResult_ShouldRenderLoginView()
        {
            // Arrange
            _mockedAuthenticationService.Setup(x => x.LogInAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(new FailureStatus<string>("Incorrent email or password."));

            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            var model = new LoginViewModel();


            // Act & Assert
            accountController.WithCallTo(c => c.Login(model))
                             .ShouldRenderDefaultView()
                             .WithModel(model)
                             .AndModelError(string.Empty)
                             .ThatEquals("Incorrent email or password.");
        }

        [Test]
        public void LogOff_ShouldRedirectToLogin()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            _mockedFormsAuthenticationWrapper.Setup(x => x.SignOut());

            // Act & Assert
            accountController.WithCallTo(c => c.LogOff())
                             .ShouldRedirectTo<AccountController>(x => x.Login());

            _mockedFormsAuthenticationWrapper.Verify(x => x.SignOut(), Times.Once);
        }

        [Test]
        public void RegisterOnGetRequest_WithDefaultFlow_ShouldRenderRegisterView()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            // Act & Assert
            accountController.WithCallTo(c => c.Register())
                             .ShouldRenderDefaultView();
        }

        [Test]
        public void RegisterOnPostRequest_WithInvalidModel_ShouldRenderRegisterView()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            accountController.ModelState.AddModelError("errorKey", "error");

            var model = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(model))
                             .ShouldRenderDefaultView()
                             .WithModel(model)
                             .AndModelError("errorKey")
                             .ThatEquals("error");
        }

        [Test]
        public void RegisterOnPostRequest_WithValidModelAndSucceededCreateResult_ShouldRedirectToAvailableCoursesView()
        {
            // Arrange
            _mockedAuthenticationService.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(new SuccessStatus<string>(string.Empty));

            _mockedFormsAuthenticationWrapper.Setup(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()));

            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            var model = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(model))
                             .ShouldRedirectTo<CourseController>(x => x.AvailableCourses());

            _mockedFormsAuthenticationWrapper.Verify(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void RegisterOnPostRequest_WithValidModelAndFailedCreateResult_ShouldRenderRegisterView()
        {
            // Arrange
            const string error = "custom error";

            _mockedAuthenticationService.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(new FailureStatus<string>(error));

            var accountController = new AccountController(_mockedAuthenticationService.Object,
                _mockedFormsAuthenticationWrapper.Object);

            var model = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(model))
                             .ShouldRenderDefaultView()
                             .WithModel(model)
                             .AndModelError("")
                             .ThatEquals(error);
        }
    }
}
