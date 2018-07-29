using System;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

using StudentSystem.Clients.Mvc.Controllers;
using StudentSystem.Clients.Mvc.ViewModels.Account;
using StudentSystem.Data.Identity;

namespace StudentSystem.Clients.Mvc.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAuthenticationService> _mockedAuthenticationService;

        [SetUp]
        public void SetUp()
        {
            _mockedAuthenticationService = new Mock<IAuthenticationService>();
        }

        [Test]
        public void Constructor_WithNullIAuthenticationService_ShouldThrowsArgumentNullException()
        {
            var ex = Assert.Catch<ArgumentNullException>(() => new AccountController(null));

            StringAssert.Contains("authenticationService", ex.Message);
        }

        [Test]
        public void LoginOnGetRequest_WithAuthenticateUser_ShouldRedirectToAvailableCourses()
        {
            // Arrange
            var mockContext = new Mock<ControllerContext>();

            mockContext.Setup(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);

            var accountController = new AccountController(_mockedAuthenticationService.Object)
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

            var accountController = new AccountController(_mockedAuthenticationService.Object)
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
            var accountController = new AccountController(_mockedAuthenticationService.Object);

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
            _mockedAuthenticationService.Setup(x => x.LogIn(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                        .ReturnsAsync(SignInStatus.Success);

            var accountController = new AccountController(_mockedAuthenticationService.Object);

            var model = new LoginViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Login(model))
                             .ShouldRedirectTo<CourseController>(x => x.AvailableCourses());
        }

        [Test]
        public void LoginOnPostRequest_WithValidModelStateAndNotSuccessResult_ShouldRenderLoginView()
        {
            // Arrange
            _mockedAuthenticationService.Setup(x => x.LogIn(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                        .ReturnsAsync(SignInStatus.Failure);

            var accountController = new AccountController(_mockedAuthenticationService.Object);

            var model = new LoginViewModel();
            

            // Act & Assert
            accountController.WithCallTo(c => c.Login(model))
                             .ShouldRenderDefaultView()
                             .WithModel(model)
                             .AndModelError(string.Empty)
                             .ThatEquals("Incorrent email or password.");
        }

        [Test]
        public void RegisterOnGetRequest_WithDefaultFlow_ShouldRenderRegisterView()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object);

            // Act & Assert
            accountController.WithCallTo(c => c.Register())
                             .ShouldRenderDefaultView();
        }

        [Test]
        public void RegisterOnPostRequest_WithInvalidModel_ShouldRenderRegisterView()
        {
            // Arrange
            var accountController = new AccountController(_mockedAuthenticationService.Object);

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
            _mockedAuthenticationService.Setup(x => x.CreateAccountAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(IdentityResult.Success);

            var accountController = new AccountController(_mockedAuthenticationService.Object);

            var model = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(model))
                             .ShouldRedirectTo<CourseController>(x => x.AvailableCourses());
        }

        [Test]
        public void RegisterOnPostRequest_WithValidModelAndFailedCreateResult_ShouldRenderRegisterView()
        {
            // Arrange
            const string error = "custom error";

            _mockedAuthenticationService.Setup(x => x.CreateAccountAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .ReturnsAsync(IdentityResult.Failed(error));

            var accountController = new AccountController(_mockedAuthenticationService.Object);

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
