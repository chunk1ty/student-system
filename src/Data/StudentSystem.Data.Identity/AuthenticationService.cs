using System;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Identity
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> CreateAccountAsync(string email, string password);

        Task<SignInStatus> LogIn(string email, string password, bool rememberMe);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityUserManagerService _identityUserManagerService;
        private readonly IIdentitySignInService _identitySignInService;

        public AuthenticationService(
            IIdentitySignInService identitySignInService,
            IIdentityUserManagerService identityUserManagerService)
        {
            _identitySignInService = identitySignInService ?? throw new ArgumentNullException(nameof(identitySignInService));
            _identityUserManagerService = identityUserManagerService ?? throw new ArgumentNullException(nameof(_identityUserManagerService));
        }

        public async Task<IdentityResult> CreateAccountAsync(string email, string password)
        {
            var userEntity = new Student()
            {
                Email = email,
                UserName = email
            };

            var result = await _identityUserManagerService.CreateAsync(userEntity, password);

            if (result.Succeeded)
            {
                await _identitySignInService.SignInAsync(userEntity, false, false);
            }

            return result;
        }

        public async Task<SignInStatus> LogIn(string email, string password, bool rememberMe){
            
            return await _identitySignInService.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = await _identityUserManagerService.ChangePasswordAsync(userId, oldPassword, newPassword);

            if (result.Succeeded)
            {
                var user = await _identityUserManagerService.FindByIdAsync(userId);

                if (user != null)
                {
                    await _identitySignInService.SignInAsync(user, false, false);
                }
            }

            return result;
        }
    }
}
