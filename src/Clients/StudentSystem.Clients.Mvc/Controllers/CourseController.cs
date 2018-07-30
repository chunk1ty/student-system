using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;
using System.Web.Mvc.Expressions;
using StudentSystem.Common.Constants;
using StudentSystem.Services.Mapping;

namespace StudentSystem.Clients.Mvc.Controllers
{
    //TODO rename methods; separation of concers 
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
            var status = await _courseService.GetAllAsync();

            if (!status.IsSuccessful)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, status.ErrorMessage);
            }

            var coursesViewModel =  _mapping.Map<IEnumerable<CourseViewModel>>(status.Result);

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

        [HttpGet]
        public async Task<ActionResult> ManageCourses()
        {
            var status = await _courseService.GetAllAsync();

            if (!status.IsSuccessful)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, status.ErrorMessage);
            }

            var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(status.Result);

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
            var status = _courseService.Add(course);

            if (status.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }

            ModelState.AddModelError(string.Empty, status.ErrorMessage);
            return View(courseAddViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var status = await _courseService.GetByIdAsync(id);

            if (status.IsSuccessful)
            {
                var courseViewModel = _mapping.Map<CourseViewModel>(status.Result);

                return View(courseViewModel);
            }

            //TODO handle error messgae ?
            return this.RedirectToAction(x => x.AvailableCourses());
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
            var status = _courseService.Update(course);

            if (status.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }
            
            ModelState.AddModelError(string.Empty, status.ErrorMessage);
            return View(courseAddViewModel);
        }

        //TODO post
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var operationStatus = await _courseService.DeleteByIdAsync(id);

            if (operationStatus.IsSuccessful)
            {
                return this.RedirectToAction(x => x.ManageCourses());
            }

            //TODO handle error messgae ?
            return this.RedirectToAction(x => x.AvailableCourses());
        }
    }
}