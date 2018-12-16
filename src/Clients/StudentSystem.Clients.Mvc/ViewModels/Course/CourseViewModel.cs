using System.ComponentModel.DataAnnotations;
using StudentSystem.Common.Constants;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.ViewModels.Course
{
    public class CourseViewModel : IMap<StudentSystem.Domain.Course>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = ClientMessage.MaxAndMinLength, MinimumLength = 2)]
        public string Name { get; set; }
    }
}