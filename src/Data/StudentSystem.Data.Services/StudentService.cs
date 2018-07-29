using System;
using System.Linq;
using System.Threading.Tasks;
using StudentSystem.Common;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    public class StudentService : IStudentService
    {
        private readonly IEntityFrameworkGenericRepository<Student> _studentRepository;
        private readonly IEntityFrameworkGenericRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork; 

        public StudentService(IEntityFrameworkGenericRepository<Student> studentRepository, IEntityFrameworkGenericRepository<Course> courseRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationStatus<bool>> IsStudentEnrolledAsync(string studentId, int courseId)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIdAsync(studentId);

                if (student.Courses.Any(c => c.Id == courseId))
                {
                    return new SuccessStatus<bool>(false);
                }

                var course = await _courseRepository.GetByIdAsync(courseId);

                student.Courses.Add(course);
                _unitOfWork.Commit();

                return new SuccessStatus<bool>(true);

            }
            catch (Exception ex)
            {
                return new FailureStatus<bool>(ex.Message);
            }
        }
    }
}
