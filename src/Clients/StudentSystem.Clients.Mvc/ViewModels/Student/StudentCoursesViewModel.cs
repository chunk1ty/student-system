using System.Collections.Generic;

using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Domain.Services.Contracts.Models;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.ViewModels.Student
{
    public class StudentCoursesViewModel : IMap<StudentCourses>
    {
        public IEnumerable<CourseViewModel> Enrolled { get; set; }

        public IEnumerable<CourseViewModel> NotEnrolled { get; set; }
    }
}