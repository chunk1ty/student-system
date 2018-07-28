using StudentSystem.Clients.Mvc.Services;

namespace StudentSystem.Clients.Mvc.ViewModels.Course
{
    public class CourseViewModel : IMap<Data.Entities.Course>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class CourseAddViewModel : IMap<Data.Entities.Course>
    {
        public string Name { get; set; }
    }
}