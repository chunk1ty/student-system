﻿using System;
using System.Linq;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Common.Logging;
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

        public async Task<OperationStatus<string>> EnrollStudentInCourseAsync(string email, int courseId)
        {
            try
            {
                var student = await _studentRepository.GetStudentByEmailAsync(email);
                if (student.Courses.Any(c => c.Id == courseId))
                {
                    return new FailureStatus<string>(ClientMessage.AlreadyEnrolledInThisCourse);
                }

                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    return new FailureStatus<string>(ClientMessage.CourseDoesNotExist);
                }

                student.Courses.Add(course);
                _unitOfWork.Commit();

                return new SuccessStatus<string>(string.Empty);
            }
            catch (Exception ex)
            {
                Log<StudentService>.Error(ex.Message, ex);

                return new FailureStatus<string>(ClientMessage.SomethingWentWrong);
            }
        }
    }
}
