using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;
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
            var status = await _courseService.GetAllAsync();
            if (!status.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, status.ErrorMessage);

                return View(new List<CourseViewModel>());
            }

            var coursesViewModel =  _mapping.Map<IEnumerable<CourseViewModel>>(status.Result);

            return View(coursesViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            var status = await _courseService.GetAllAsync();
            if (!status.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, status.ErrorMessage);

                return View(new List<CourseViewModel>());
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
                return this.RedirectToAction(x => x.Manage());
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

            ModelState.AddModelError(string.Empty, status.ErrorMessage);

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
            var status = _courseService.Update(course);

            if (status.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Manage());
            }
            
            ModelState.AddModelError(string.Empty, status.ErrorMessage);
            return View(courseAddViewModel);
        }
       
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var status = await _courseService.DeleteByIdAsync(id);
            if (status.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Manage());
            }

            ModelState.AddModelError(string.Empty, status.ErrorMessage);

            return this.RedirectToAction(x => x.Index());
        }
    }
}