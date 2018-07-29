﻿using System;
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
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Course>> GetAllByStudentIdAsync(string studentid)
        {
            return await _courseRepository.GetAllByStudentIdAsync(studentid);
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public OperationStatus<Course> Add(Course course)
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
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<Course>(ex.Message);
            }

            return new SuccessStatus<Course>(course);
        }

        public async Task<OperationStatus<int>> DeleteAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                _courseRepository.Delete(course);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<int>(ex.Message);
            }

            return new SuccessStatus<int>(id);
        }

        public OperationStatus<Course> Update(Course course)
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
                Log<CourseService>.Error(ex.Message, ex);

                return new FailureStatus<Course>(ex.Message);
            }

            return new SuccessStatus<Course>(course);
        }
    }
}
