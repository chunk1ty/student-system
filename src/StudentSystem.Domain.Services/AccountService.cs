//using System;
//using System.Data.SqlClient;
//using System.Data.SqlTypes;
//using System.Threading.Tasks;

//using StudentSystem.Common;
//using StudentSystem.Common.Constants;
//using StudentSystem.Persistence.Contracts;
//using StudentSystem.Domain.Services.Contracts;
//using StudentSystem.Infrastructure.RetryPolicy;
//using StudentSystem.Infrastructure.Security;

//namespace StudentSystem.Domain.Services
//{
//    //TODO naming convension "Service" ?
//    public class AccountService : IAccountService
//    {
//        private readonly IStudentRepository _studentRepository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ICypher _cypher;
//        private readonly IRetryPolicy _policy;

//        public AccountService(
//            IStudentRepository studentRepository, 
//            IUnitOfWork unitOfWork, 
//            ICypher cypher, 
//            IRetryPolicy policy)
//        {
//            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
//            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
//            _cypher = cypher ?? throw new ArgumentNullException(nameof(cypher));
//            _policy = policy;
//        }

//        //TODO SqlException -> reference to System.Data 
//        public async Task<OperationStatus<string>> RegisterAsync(string email, string password)
//        {
//            // TODO Ank : configure retry and delay 
//            // TODO try catch repeating block 
//            try
//            {
//                var result = await _policy.ExecuteWithDelayAsync<OperationStatus<string>, SqlNullValueException>(() => ExecuteRegisterAsync(email, password),
//                    3, TimeSpan.FromSeconds(3));

//                return result;
//            }
//            catch (SqlNullValueException)
//            {
//                return new FailureStatus<string>("Connectivity issues. Please try later again.");
//            }
//        }

//        public async Task<OperationStatus<string>> LogInAsync(string email, string password)
//        {
//            if (email == null)
//            {
//                throw new ArgumentNullException(nameof(email));
//            }

//            if (password == null)
//            {
//                throw new ArgumentNullException(nameof(password));
//            }

//            Student user;
//            try
//            {
//                user = await _policy.ExecuteWithDelayAsync<Student, SqlException>(() => _studentRepository.GetStudentWithCoursesByEmailAsync(email),
//                    3, TimeSpan.FromSeconds(3));
//            }
//            catch (SqlException)
//            {
//                return new FailureStatus<string>("Connectivity issues. Please try later again.");
//            }
           
//            if (user == null)
//            {
//                return new FailureStatus<string>(string.Format(ClientMessage.UserDoesNotExist, email));
//            }

//            if (!_cypher.IsPasswordMatch(password, user.Password))
//            {
//                return new FailureStatus<string>(string.Format(ClientMessage.PasswordNotRecognised, email));
//            }

//            return new SuccessStatus<string>(email);
//        }

//        // TODO OperationStatus result ?
//        private async Task<OperationStatus<string>> ExecuteRegisterAsync(string email, string password)
//        {
//            //throw new SqlNullValueException();
//            if (email == null)
//            {
//                throw new ArgumentNullException(nameof(email));
//            }

//            if (password == null)
//            {
//                throw new ArgumentNullException(nameof(password));
//            }

//            //TODO specific query to DB ?
//            var user = await _studentRepository.GetStudentWithCoursesByEmailAsync(email);
//            if (user != null)
//            {
//                // TODO return message or code and map in the UI 
//                return new FailureStatus<string>(string.Format(ClientMessage.UserAlreadyExist, email));
//            }

//            var encryptPassword = _cypher.Encrypt(password);

//            _studentRepository.Add(new Student { Email = email, Password = encryptPassword });
//            _unitOfWork.Commit();

//           return new SuccessStatus<string>(email);
//        }
//    }
//}
