using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services
{
    public class CourseService : ICourseService
    {
        private readonly IEntityFrameworkGenericRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IEntityFrameworkGenericRepository<Course> courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public OperationStatus Add(Course course)
        {
            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _courseRepository.Add(course);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                // TODO add logging
                return new FailureStatus(ex.Message);
            }

            return new SuccessStatus();
        }

        public async Task<OperationStatus> DeleteAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                _courseRepository.Delete(course);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                // TODO add logging
                return new FailureStatus(ex.Message);
            }

            return new SuccessStatus();
        }

        public OperationStatus Update(Course course)
        {
            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _courseRepository.Update(course);

                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                // TODO add logging
                return new FailureStatus(ex.Message);
            }

            return new SuccessStatus();
        }
    }
}
