using System.Threading.Tasks;
using StudentSystem.Common;

namespace StudentSystem.Data.Services.Contracts
{
    public interface IAccountService
    {
        OperationStatus<string> CreateAsync(string email, string password);

        Task<OperationStatus<string>> LogInAsync(string email, string password);
    }
}
