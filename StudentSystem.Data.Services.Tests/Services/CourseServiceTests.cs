using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
    public sealed class TestDbConfiguration : DbMigrationsConfiguration<StudentSystemDbContext>
    {
        public TestDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }

    [TestFixture]
    public class CourseServiceTests
    {
        private static IKernel _kernel;

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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, TestDbConfiguration>());


            _kernel = NinjectConfig.CreateKernel();
            var dbContext = _kernel.Get<StudentSystemDbContext>();

            foreach (var course in Courses)
            {
                dbContext.Courses.Add(course);
            }

            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            var dbContext = _kernel.Get<StudentSystemDbContext>();

            foreach (var course in Courses)
            {
                dbContext.Courses.Attach(course);
                dbContext.Courses.Remove(course);

                foreach (var student in course.Students)
                {
                    dbContext.Students.Attach(student);
                    dbContext.Students.Remove(student);
                }
            }

            dbContext.SaveChanges();
        }

        [Test]
        public async Task GetClassWithStudentsByClassIdAsync_WithValidClassId_ReturnClass()
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
    }
}
