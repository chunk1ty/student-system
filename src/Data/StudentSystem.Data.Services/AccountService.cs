using System;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
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
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cypher = cypher ?? throw new ArgumentNullException(nameof(cypher));
        }

        // TODO OperationStatus result ?
        public async Task<OperationStatus<string>> RegisterAsync(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            //TODO specific query to DB ?
            var user = await _studentRepository.GetStudentWithCoursesByEmailAsync(email);
            if (user != null)
            {
                // TODO return message or code and map in the UI 
                return new FailureStatus<string>(string.Format(ClientMessage.UserAlreadyExist, email));
            }

            var encryptPassword = _cypher.Encrypt(password);

            _studentRepository.Add(new Student { Email = email, Password = encryptPassword });
            _unitOfWork.Commit();

            return new SuccessStatus<string>(email);
        }

        public async Task<OperationStatus<string>> LogInAsync(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await _studentRepository.GetStudentWithCoursesByEmailAsync(email);
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
    }
}
