using System;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Identity
{
    public class ApplicationUserManager : UserManager<Student>, IIdentityUserManagerService
    {
        public ApplicationUserManager(IUserStore<Student> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<Student>(context.Get<StudentSystemDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Student>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(25);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
           
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Student>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }
}
