using System.Threading.Tasks;
using StudentSystem.Common;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public OperationStatus<string> CreateAsync(string email, string password)
        {
            _studentRepository.Add(new Student() {Email = email, Password = password});

            _unitOfWork.Commit();

            return new SuccessStatus<string>("Successfully registered");
        }

        public async Task<OperationStatus<string>> LogInAsync(string email, string password)
        {
            var user = await  _studentRepository.GetStudentByEmailAsync(email);

            if (user == null)
            {
                return new FailureStatus<string>($"User with email '{email}' doesn't exist");
            }

            if (!user.Password.Equals(password))
            {
                return new FailureStatus<string>($"Password for user '{email}' is not recognised.");
            }

            return new SuccessStatus<string>("Log in successfully");
        }
    }
}
