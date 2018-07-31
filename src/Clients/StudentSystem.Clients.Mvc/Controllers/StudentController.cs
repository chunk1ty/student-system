using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using StudentSystem.Clients.Mvc.ViewModels.Student;
using StudentSystem.Common.Constants;
using StudentSystem.Data.Services.Contracts;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IMappingService _mapping;

        public StudentController(IStudentService studentService, IMappingService mapping)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        [HttpGet]
        public async Task<ActionResult> Courses()
        {
            var status = await _studentService.GetStudentCourses(User.Identity.Name);
            if (!status.IsSuccessful)
            {
                TempData["Error"] = status.ErrorMessage;

                return this.RedirectToAction<CourseController>(x => x.Index());
            }

            var coursesViewModel = _mapping.Map<StudentCoursesViewModel>(status.Result);

            return View(coursesViewModel);
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
    }
}