using System;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.Owin;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Identity
{
    public interface IIdentitySignInService : IDisposable
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        Task SignInAsync(Student user, bool isPersistent, bool rememberBrowser);
    }
}