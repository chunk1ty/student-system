using System.Data.Entity;
using System.Threading.Tasks;

using StudentSystem.Persistence.Contracts;
using StudentSystem.Domain;
using StudentSystem.Infrastructure.Mapping;
using StudentSystem.Persistence.Entities;

namespace StudentSystem.Persistence.Repositories
{
    public class EfStudentRepository : IStudentRepository
    {
        private readonly IMappingService _mapping;
        private readonly StudentSystemDbContext _studentSystemDbContext;

        public EfStudentRepository(StudentSystemDbContext studentSystemDbContext, IMappingService mapping)
        {
            _studentSystemDbContext = studentSystemDbContext;
            _mapping = mapping;
        }

        public async Task<Student> GetStudentWithCoursesByEmailAsync(string email)
        {
            var student = await _studentSystemDbContext.Set<StudentEntity>()
                .AsNoTracking()
                                                .Include(x => x.Courses)
                                                .SingleOrDefaultAsync(x => x.Email.Equals(email));

            return _mapping.Map<Student>(student);
        }

        public void Add(Student student)
        {
            var entity = _mapping.Map<StudentEntity>(student);
            //var entry = _studentSystemDbContext.Entry(entity);

            //if (entry.State != EntityState.Detached)
            //{
            //    entry.State = EntityState.Added;
            //}
            //else
            //{
                _studentSystemDbContext.Students.Add(entity);

            //_studentSystemDbContext.Students
            //}
        }

        public void EnrollToCourse(Student student, Course course)
        {
            var studentEntity = _mapping.Map<StudentEntity>(student);
            _studentSystemDbContext.Students.Attach(studentEntity);

            var courseEntity = _mapping.Map<CourseEntity>(course);
            _studentSystemDbContext.Courses.Attach(courseEntity);

            studentEntity.Courses.Add(courseEntity);
        }
    }
}
