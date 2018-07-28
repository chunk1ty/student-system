using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentSystem.Clients.Mvc.Services;
using StudentSystem.Clients.Mvc.ViewModels.Course;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Services.Contracts;
using System.Web.Mvc.Expressions;

namespace StudentSystem.Clients.Mvc.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMappingService _mapping;

        public CourseController(ICourseService courseService, IMappingService mapping)
        {
            _courseService = courseService;
            _mapping = mapping;
        }

       [HttpGet]
        public async Task<ActionResult> Index()
       {
           var courses = await _courseService.GetAllAsync();

           var coursesViewModel = _mapping.Map<IEnumerable<CourseViewModel>>(courses);

            return View(coursesViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseAddViewModel courseAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(courseAddViewModel);
            }

            var course = _mapping.Map<Course>(courseAddViewModel);

            var operationStatus = _courseService.Add(course);

            if (operationStatus.IsSuccessful)
            {
                return this.RedirectToAction(x => x.Index());
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
                return this.RedirectToAction(x => x.Index());
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
                return this.RedirectToAction(x => x.Index());
            }

            // TODO add some error to modelstate 

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
        }
    }
}