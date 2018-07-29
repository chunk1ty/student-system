using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Identity
{
    public interface IIdentityUserManagerService
    {
        Task<IdentityResult> CreateAsync(Student user, string password);
    }
}