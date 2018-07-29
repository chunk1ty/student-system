using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using StudentSystem.Clients.Mvc.Services;
using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;
using System.Web.Mvc.Expressions;

using Microsoft.AspNet.Identity;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly IMappingService _mapping;

        public CourseController(ICourseService courseService, IMappingService mapping, IStudentService studentService)
        {
            _courseService = courseService;
            _mapping = mapping;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult> AvailableCourses()
        {
            var coursesViewModel = await GetAllCourses();

            return View(coursesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(int courseId)
        {
            var operationStatus =  await _studentService.IsStudentEnrolledAsync(User.Identity.GetUserId(), courseId);

            if (!operationStatus.IsSuccessful)
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Content(operationStatus.Result ? "Successfully enrolled!" : "You are already enrolled in this course!");
        }

        [HttpGet]
        public async Task<ActionResult> EnrolledCourses()
        {
            var courses = await _courseService.GetAllByStudentIdAsync(User.Identity.GetUserId());

            var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(courses);

            return View(coursesViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ManageCourses()
        {
            var coursesViewModel = await GetAllCourses();

            return View(coursesViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel courseAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(courseAddViewModel);
            }

            var course = _mapping.Map<Course>(courseAddViewModel);

            var operationStatus = _courseService.Add(course);

            if (operationStatus.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }

            // TODO add some error to modelstate 

            return View(courseAddViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);

            var courseViewModel = _mapping.Map<CourseViewModel>(course);

            return View(courseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseViewModel courseAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(courseAddViewModel);
            }

            var course = _mapping.Map<Course>(courseAddViewModel);

            var operationStatus = _courseService.Update(course);

            if (operationStatus.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }

            // TODO add some error to modelstate 

            return View(courseAddViewModel);
        }

        //TODO post
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var operationStatus = await _courseService.DeleteAsync(id);

            if (operationStatus.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }

            // TODO add some error to modelstate 

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private async Task<IEnumerable<CourseViewModel>> GetAllCourses()
        {
            var courses = await _courseService.GetAllAsync();

            return _mapping.Map<IEnumerable<CourseViewModel>>(courses);
        }
    }
}