using System.Collections.Generic;
using StudentSystem.Domain;

namespace StudentSystem.Domain.Services.Contracts.Models
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
