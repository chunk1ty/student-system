using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace StudentSystem.Data.Identity
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> CreateAccountAsync(string email, string password);

        Task<SignInStatus> LogIn(string email, string password, bool rememberMe);
    }
}