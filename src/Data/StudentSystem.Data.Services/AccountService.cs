using System;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Common.Logging;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;
using StudentSystem.Infrastructure.Security;

namespace StudentSystem.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICypher _cypher; 

        public AccountService(
            IStudentRepository studentRepository, 
            IUnitOfWork unitOfWork, 
            ICypher cypher)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _cypher = cypher;
        }

        public async Task<OperationStatus<string>> RegisterAsync(string email, string password)
        {
            try
            {
                var user = await _studentRepository.GetStudentByEmailAsync(email);
                if (user != null)
                {
                    return new FailureStatus<string>(string.Format(ClientMessage.UserAlreadyExist, email));
                }

                var encryptPassword = _cypher.Encrypt(password);

                _studentRepository.Add(new Student { Email = email, Password = encryptPassword });
                _unitOfWork.Commit();

                return new SuccessStatus<string>(email);
            }
            catch (Exception ex)
            {
                Log<AccountService>.Error(ex.Message, ex);

                return new FailureStatus<string>(ClientMessage.SomethingWentWrong);
            }
        }

        public async Task<OperationStatus<string>> LogInAsync(string email, string password)
        {
            try
            {
                var user = await _studentRepository.GetStudentByEmailAsync(email);
                if (user == null)
                {
                    return new FailureStatus<string>(string.Format(ClientMessage.UserDoesNotExist, email));
                }
                if (!_cypher.IsPasswordMatch(password, user.Password))
                {
                    return new FailureStatus<string>(string.Format(ClientMessage.PasswordNotRecognised, email));
                }

                return new SuccessStatus<string>(email);
            }
            catch (Exception ex)
            {
                Log<AccountService>.Error(ex.Message, ex);

                return new FailureStatus<string>(ClientMessage.SomethingWentWrong);
            }
        }
    }
}
