using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSystem.Common;

namespace StudentSystem.Authentication
{
    public interface IAuthenticationService
    {
        Task<OperationStatus<string>> LogInAsync(string email, string password);

        Task<OperationStatus<string>> RegisterAsync(string email, string password);
    }
}
