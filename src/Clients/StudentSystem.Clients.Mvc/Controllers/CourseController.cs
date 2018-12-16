using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Domain;
using StudentSystem.Domain.Services.Contracts;
using System.Web.Mvc.Expressions;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMappingService _mapping;

        public CourseController(ICourseService courseService, IMappingService mapping)
        {
            _courseService = courseService ?? throw  new ArgumentNullException(nameof(courseService));
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var error = (string)TempData["Error"];
            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError(string.Empty, error);
            }

            var operation = await _courseService.GetAllAsync();
            if (operation.IsSuccessful)
            {
                var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(operation.Result);

                return View(coursesViewModel);
            }

            ModelState.AddModelError(string.Empty, operation.ErrorMessage);
            return View(new List<CourseViewModel>());
        }

        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            var operation = await _courseService.GetAllAsync();
            if (operation.IsSuccessful)
            {
                var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(operation.Result);

                return View(coursesViewModel);
            }

            TempData["Error"] = operation.ErrorMessage;
            return this.RedirectToAction(x => x.Index());
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
            var operation = _courseService.Add(course);
            if (operation.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Manage());
            }

            ModelState.AddModelError(string.Empty, operation.ErrorMessage);
            return View(courseAddViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var operation = await _courseService.GetByIdAsync(id);
            if (operation.IsSuccessful)
            {
                var courseViewModel = _mapping.Map<CourseViewModel>(operation.Result);

                return View(courseViewModel);
            }

            TempData["Error"] = operation.ErrorMessage;
            return this.RedirectToAction(x => x.Index());
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
            var operation = _courseService.Update(course);
            if (operation.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Manage());
            }
            
            ModelState.AddModelError(string.Empty, operation.ErrorMessage);
            return View(courseAddViewModel);
        }
       
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var operation = await _courseService.DeleteByIdAsync(id);
            if (operation.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Manage());
            }

            TempData["Error"] = operation.ErrorMessage;
            return this.RedirectToAction(x => x.Index());
        }
    }
}