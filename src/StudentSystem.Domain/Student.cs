using System.Collections.Generic;

namespace StudentSystem.Domain
{
    public class Student
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
