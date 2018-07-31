using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Common.Constants;
using StudentSystem.Common.Logging;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
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

        public async Task<OperationStatus<IEnumerable<Course>>> GetAllAsync()
        {
            try
            {
                var courses = await _courseRepository.GetAllAsync();

                return new SuccessStatus<IEnumerable<Course>>(courses);
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<IEnumerable<Course>>(ClientMessage.SomethingWentWrong);
            }
        }

        public async Task<OperationStatus<Course>> GetByIdAsync(int id)
        {
            try
            {
                var courses = await _courseRepository.GetByIdAsync(id);

                return new SuccessStatus<Course>(courses);
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<Course>(ClientMessage.SomethingWentWrong);
            }
        }

        public OperationStatus<Course> Add(Course course)
        {
            try
            {
                if (course == null)
                {
                    return new FailureStatus<Course>(ClientMessage.CourseCannotBeNull);
                }

                _courseRepository.Add(course);
                _unitOfWork.Commit();

                return new SuccessStatus<Course>(course);
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<Course>(ClientMessage.SomethingWentWrong);
            }
        }

        public async Task<OperationStatus<int>> DeleteByIdAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    return new FailureStatus<int>(ClientMessage.CourseDoesNotExist);
                }

                _courseRepository.Delete(course);
                _unitOfWork.Commit();

                return new SuccessStatus<int>(id);
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<int>(ClientMessage.SomethingWentWrong);
            }
        }

        public OperationStatus<Course> Update(Course course)
        {
            try
            {
                if (course == null)
                {
                    return new FailureStatus<Course>(ClientMessage.CourseCannotBeNull);
                }

                _courseRepository.Update(course);
                _unitOfWork.Commit();

                return new SuccessStatus<Course>(course);
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<Course>(ClientMessage.SomethingWentWrong);
            }
        }
    }
}
