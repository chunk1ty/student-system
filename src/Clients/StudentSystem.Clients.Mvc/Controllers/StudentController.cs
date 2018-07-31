using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Common.Constants;
using StudentSystem.Data.Services.Contracts;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IMappingService _mapping;

        public StudentController(IStudentService studentService, ICourseService courseService, IMappingService mapping)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(int courseId)
        {
            var status = await _studentService.EnrollStudentInCourseAsync(User.Identity.Name, courseId);

            if (status.IsSuccessful)
            {
                return Content(ClientMessage.SuccessfullyEnrolled);
            }

            return Content(status.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> EnrolledCourses()
        {
            var status = await _courseService.GetAllByStudentEmailAsync(User.Identity.Name);

            if (!status.IsSuccessful)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, status.ErrorMessage);
            }

            var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(status.Result);

            return View(coursesViewModel);
        }
    }
}