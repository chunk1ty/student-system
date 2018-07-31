﻿using System.Collections.Generic;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Services.Contracts.Models
{
    public class StudentCourses
    {
        public StudentCourses(IEnumerable<Course> enrolled, IEnumerable<Course> notEnrolled)
        {
            Enrolled = enrolled;
            NotEnrolled = notEnrolled;
        }

        public IEnumerable<Course> Enrolled { get; }

        public IEnumerable<Course> NotEnrolled { get; }
    }
}