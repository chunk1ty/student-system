using System;
using System.Linq;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Persistence.Contracts;
using StudentSystem.Domain.Services.Contracts;
using StudentSystem.Domain.Services.Contracts.Models;

namespace StudentSystem.Domain.Services
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

        public async Task<OperationStatus<string>> EnrollStudentInCourseAsync(string email, int courseId)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (courseId <= 0 )
            {
                throw new ArgumentNullException("id cannot be less or equal to 0");
            }

            var student = await _studentRepository.GetStudentWithCoursesByEmailAsync(email);
            if (student.Courses.Any(c => c.Id == courseId))
            {
                return new FailureStatus<string>(ClientMessage.AlreadyEnrolledInThisCourse);
            }

            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                //TODO exception or FailureStatus ?
                return new FailureStatus<string>(ClientMessage.CourseDoesNotExist);
            }

            student.Courses.Add(course);
            _unitOfWork.Commit();

            return new SuccessStatus<string>(string.Empty);
        }

        public async Task<OperationStatus<StudentCourses>> GetStudentCourses(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var student = await _studentRepository.GetStudentWithCoursesByEmailAsync(email);

            var courses = await _courseRepository.GetAllAsync();

            var enrolledCoursesIds = student.Courses.Select(x => x.Id)
                                                    .ToList();
            var notEnrolledCourses = courses.Where(x => !enrolledCoursesIds.Contains(x.Id));

            //TODO StudentCourses is it in the correct assembly ?
            return new SuccessStatus<StudentCourses>(new StudentCourses(student.Courses, notEnrolledCourses));
        }
    }
}
