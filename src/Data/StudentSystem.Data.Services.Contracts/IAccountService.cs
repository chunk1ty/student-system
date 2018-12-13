using System.Threading.Tasks;

using StudentSystem.Common;

namespace StudentSystem.Data.Services.Contracts
{
    public interface IAccountService
    {
        Task<OperationStatus<string>> RegisterAsync(string email, string password);

        Task<OperationStatus<string>> LogInAsync(string email, string password);
    }
}
