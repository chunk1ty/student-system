using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ninject;
using NUnit.Framework;

using StudentSystem.Clients.Mvc;
using StudentSystem.Domain;
using StudentSystem.Domain.Services.Contracts;
using StudentSystem.Persistence;

namespace StudentSystem.Data.Services.Tests.Services
{
    // TODO add more tests
    [TestFixture, Rollback]
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
            var operation = await claaService.GetAllAsync();

            // Assert
            Assert.AreEqual(3, operation.Result.Count());
            Assert.IsTrue(operation.IsSuccessful);
        }

        [Test]
        public async Task GetByIdAsync_WhenCourseIdExists_ShouldReturnCourse()
        {
            // Arrange
            var course = Courses[0];

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var operation = await claaService.GetByIdAsync(course.Id);

            // Assert
            Assert.AreEqual("Math", operation.Result.Name);
            Assert.IsTrue(operation.IsSuccessful);
        }

        [Test]
        public async Task GetByIdAsync_WhenCourseIdDoesNotExit_ShouldReturnNull()
        {
            // Arrange
            var courseId = -1;

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var operation = await claaService.GetByIdAsync(courseId);

            // Assert
           Assert.IsNull(operation.Result);
           Assert.IsTrue(operation.IsSuccessful);
        }
    }
}
