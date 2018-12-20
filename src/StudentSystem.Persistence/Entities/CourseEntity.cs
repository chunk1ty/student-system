using System.Collections.Generic;
using StudentSystem.Domain;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Persistence.Entities
{
    public  class CourseEntity : IMap<Course>
    {
        public CourseEntity()
        {
            Students = new HashSet<StudentEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<StudentEntity> Students { get; set; }
    }
}
