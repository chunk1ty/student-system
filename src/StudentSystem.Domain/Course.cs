using System.Collections.Generic;

namespace StudentSystem.Domain
{
    public class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
