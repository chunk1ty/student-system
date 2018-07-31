using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ninject;
using NUnit.Framework;

using StudentSystem.Clients.Mvc;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services.Tests.Services
{
    // TODO add more tests
    [TestFixture]
    public class CourseServiceTests
    {
        private IKernel _kernel;

        private static readonly List<Course> Courses = new List<Course>()
        {
            new Course
            {
                Name = "Math"
            },
            new Course
            {
                Name = "English"
            },
            new Course
            {
                Name = "Biology"
            }
        };

        [SetUp]
        public void SetUp()
        {
            _kernel = NinjectConfig.CreateKernel();
            var studentSystemDbContext = _kernel.Get<StudentSystemDbContext>();

            foreach (var course in Courses)
            {
                studentSystemDbContext.Courses.Add(course);
            }

            studentSystemDbContext.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_WithDefaultFlow_ShouldReturnAllCourses()
        {
            // Arrange
            var claaService = _kernel.Get<ICourseService>();

            // Act
            var status = await claaService.GetAllAsync();

            // Assert
            Assert.AreEqual(3, status.Result.Count());
            Assert.IsTrue(status.IsSuccessful);
        }

        [Test]
        public async Task GetByIdAsync_WhenCourseIdExists_ShouldReturnCourse()
        {
            // Arrange
            var course = Courses[0];

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var status = await claaService.GetByIdAsync(course.Id);

            // Assert
            Assert.AreEqual("Math", status.Result.Name);
            Assert.IsTrue(status.IsSuccessful);
        }

        [Test]
        public async Task GetByIdAsync_WhenCourseIdDoesNotExit_ShouldReturnNull()
        {
            // Arrange
            var courseId = -1;

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var status = await claaService.GetByIdAsync(courseId);

            // Assert
           Assert.IsNull(status.Result);
           Assert.IsTrue(status.IsSuccessful);
        }
    }
}
