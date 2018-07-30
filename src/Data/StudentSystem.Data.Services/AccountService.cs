using System;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    //TODO add encryption
    public class AccountService : IAccountService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
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

                _studentRepository.Add(new Student { Email = email, Password = password });
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
                if (!user.Password.Equals(password))
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
