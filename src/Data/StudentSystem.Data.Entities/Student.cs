using System.Collections.Generic;

namespace StudentSystem.Data.Entities
{
    //TODO immutable class
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
