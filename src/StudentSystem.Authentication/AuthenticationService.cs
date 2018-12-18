using System;
using System.Data.Entity;
using System.Threading.Tasks;
using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Infrastructure.Security;

namespace StudentSystem.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICypher _cypher;
        private readonly StudentSystemAuthDbContext _studentSystemAuthDbContext;

        public AuthenticationService(StudentSystemAuthDbContext studentSystemAuthDbContext, ICypher cypher)
        {
            _studentSystemAuthDbContext = studentSystemAuthDbContext;
            _cypher = cypher;
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

            var user = await _studentSystemAuthDbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

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
            var user = await _studentSystemAuthDbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                // TODO return message or code and map in the UI 
                return new FailureStatus<string>(string.Format(ClientMessage.UserAlreadyExist, email));
            }

            var encryptPassword = _cypher.Encrypt(password);

            _studentSystemAuthDbContext.Users.Add(new User { Email = email, Password = encryptPassword });

            _studentSystemAuthDbContext.SaveChanges();

            return new SuccessStatus<string>(email);
        }
    }
}
