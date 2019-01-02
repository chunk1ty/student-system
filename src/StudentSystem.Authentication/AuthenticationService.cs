using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Domain;
using StudentSystem.Domain.Services.Contracts;
using StudentSystem.Infrastructure.Security;

namespace StudentSystem.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        //TODO wrap external libraries ?
        private readonly IMediator _mediator;
        private readonly ICypher _cypher;
        private readonly StudentSystemAuthDbContext _studentSystemAuthDbContext;

        public AuthenticationService(StudentSystemAuthDbContext studentSystemAuthDbContext, ICypher cypher, IMediator mediator)
        {
            _studentSystemAuthDbContext = studentSystemAuthDbContext;
            _cypher = cypher;
            _mediator = mediator;
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
         
            await _mediator.Publish(new AccountCreated(email,"FirstName " + email, "lastName" + email, Role.Student));

            return new SuccessStatus<string>(email);
        }
    }

    public class AccountCreated : INotification
    {
        public AccountCreated(string email, string firstName, string lastName, Role role)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        public string Email { get; }

        public string FirstName { get;  }

        public string LastName { get; }

        public Role Role { get; }
    }

    public enum Role
    {
        Student,
        Teacher
    }

    public class StudentCreatedHandler : INotificationHandler<AccountCreated>
    {
        private readonly IStudentService _studentService;

        public StudentCreatedHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
        {
            if (notification.Role != Role.Student)
            {
                return;
            }

            _studentService.Add(new Student() {Email = notification.Email, FirstName = "Andriyan", LastName = "Krastev"});
        }
    }
}
