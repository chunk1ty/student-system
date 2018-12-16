using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;

using StudentSystem.Clients.Mvc.ViewModels.Student;
using StudentSystem.Common.Constants;
using StudentSystem.Domain.Services.Contracts;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
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
            var operation = await _studentService.GetStudentCourses(User.Identity.Name);
            if (operation.IsSuccessful)
            {
                var coursesViewModel = _mapping.Map<StudentCoursesViewModel>(operation.Result);

                return View(coursesViewModel);
            }

            TempData["Error"] = operation.ErrorMessage;

            return this.RedirectToAction<CourseController>(x => x.Index());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(int courseId)
        {
            var operation = await _studentService.EnrollStudentInCourseAsync(User.Identity.Name, courseId);
            if (operation.IsSuccessful)
            {
                return Content(ClientMessage.SuccessfullyEnrolled);
            }

            return Content(operation.ErrorMessage);
        }
    }
}