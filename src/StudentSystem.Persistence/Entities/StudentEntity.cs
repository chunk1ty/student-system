using System.Collections.Generic;
using StudentSystem.Domain;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Persistence.Entities
{
    public  class StudentEntity : IMap<Student>
    {
        public StudentEntity()
        {
            Courses = new HashSet<CourseEntity>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<CourseEntity> Courses { get; set; }
    }
}
