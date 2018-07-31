using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ninject;
using NUnit.Framework;

using StudentSystem.Clients.Mvc;
using StudentSystem.Common;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;

namespace StudentSystem.Data.Services.Tests.Services
{
    [TestFixture, Rollback]
    public class CourseServiceTests
    {
        private IKernel _kernel;

        private static readonly List<Course> Courses = new List<Course>()
        {
            new Course
            {
                Name = "Math",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        Email = "ankk@ankk.com",
                        Password = "123456"
                    }
                }
            },
            new Course
            {
                Name = "English",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        Email = "test@ankk.com",
                        Password = "123456"
                    }
                }
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
        public async Task GetAllByStudentEmailAsync_WithValidEmail_ShouldReturnCourses()
        {
            // Arrange
            const string email = "ankk@ankk.com";

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var courses = await claaService.GetAllByStudentEmailAsync(email);

            // Assert
            Assert.IsInstanceOf<OperationStatus<IEnumerable<Course>>>(courses);
            Assert.AreEqual(1, courses.Result.Count());
            Assert.AreEqual("Math", courses.Result.First().Name);
        }

        [Test]
        public async Task GetAllByStudentEmailAsync_WithInValidEmail_ShouldReturnEmpty()
        {
            // Arrange
            const string email = "nothing@ankk.com";

            var claaService = _kernel.Get<ICourseService>();

            // Act
            var courses = await claaService.GetAllByStudentEmailAsync(email);

            // Assert
            Assert.IsInstanceOf<OperationStatus<IEnumerable<Course>>>(courses);
            Assert.AreEqual(0, courses.Result.Count());
        }

        //TODO add move tests if i have time
    }
}
