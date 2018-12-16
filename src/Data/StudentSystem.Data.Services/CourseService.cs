using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Data.Contracts;
using StudentSystem.Domain;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        //TODO return value is only success ?
        public async Task<OperationStatus<IEnumerable<Course>>> GetAllAsync()
        {
            var courses = await _courseRepository.GetAllAsync();

            return new SuccessStatus<IEnumerable<Course>>(courses);
        }

        public async Task<OperationStatus<Course>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id cannot be less or equal to 0");
            }

            var courses = await _courseRepository.GetByIdAsync(id);

            return new SuccessStatus<Course>(courses);
        }

        public OperationStatus<Course> Add(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            _courseRepository.Add(course);
            _unitOfWork.Commit();

            return new SuccessStatus<Course>(course);
        }

        public async Task<OperationStatus<int>> DeleteByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id cannot be less or equal to 0");
            }

            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new FailureStatus<int>(ClientMessage.CourseDoesNotExist);
            }

            _courseRepository.Delete(course);
            _unitOfWork.Commit();

            return new SuccessStatus<int>(id);
        }

        public OperationStatus<Course> Update(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            _courseRepository.Update(course);
            _unitOfWork.Commit();

            return new SuccessStatus<Course>(course);
        }
    }
}
