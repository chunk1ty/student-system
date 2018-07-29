using System.ComponentModel.DataAnnotations;
using StudentSystem.Clients.Mvc.Services;

namespace StudentSystem.Clients.Mvc.ViewModels.Course
{
    public class CourseViewModel : IMap<Data.Entities.Course>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}