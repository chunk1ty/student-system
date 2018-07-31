namespace StudentSystem.Common.Constants
{
    // These messages should be in resource file
    public class ClientMessage
    {
        public const string UserDoesNotExist = "User with email '{0}' doesn't exist.";

        public const string UserAlreadyExist = "User with email '{0}' already exists.";

        public const string PasswordNotRecognised = "Password for user '{0}' is not recognised.";

        public const string CourseDoesNotExist = "Course doesn't exist.";

        public const string CourseCannotBeNull = "Course cannot be null.";

        public const string SuccessfullyEnrolled = "Successfully enrolled.";

        public const string AlreadyEnrolledInThisCourse = "You are already enrolled in this course!";

        public const string SomethingWentWrong = "Something went wrong. Please try again or contact your administrator";

        public const string PasswordLength = "The {0} must be at least {2} characters long.";

        public const string Email = "Email";

        public const string Password = "Password";

        public const string RememberMe = "Remember me?";

        public const string PasswordDoesNotMatch = "The password and confirmation password do not match.";

        public const string ConfirmPassword = "Confirm password.";
    }
}
