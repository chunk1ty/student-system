﻿using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;

namespace StudentSystem.Domain.Services.Contracts
{
    public interface ICourseService
    {
        Task<OperationStatus<IEnumerable<Course>>> GetAllAsync();

        Task<OperationStatus<Course>> GetByIdAsync(int id);

        OperationStatus<Course> Add(Course course);

        OperationStatus<int> DeleteByIdAsync(int id);

        OperationStatus<Course> Update(Course course);
    }
}
