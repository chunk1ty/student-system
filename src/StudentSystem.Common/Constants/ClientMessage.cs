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
    }
}
