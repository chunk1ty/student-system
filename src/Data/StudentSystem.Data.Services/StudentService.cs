using System;
using System.Linq;
using System.Threading.Tasks;

using StudentSystem.Common;

using StudentSystem.Data.Contracts;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork; 

        public StudentService(
            IStudentRepository studentRepository, 
            ICourseRepository courseRepository, 
            IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository ?? throw  new ArgumentNullException(nameof(courseRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<OperationStatus<bool>> EnrollStudentInCourseAsync(string studentId, int courseId)
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
