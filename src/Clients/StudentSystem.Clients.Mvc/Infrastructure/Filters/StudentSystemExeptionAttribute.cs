using System.Web.Mvc;

using StudentSystem.Common.Logging;

namespace StudentSystem.Clients.Mvc.Infrastructure.Filters
{
    public class StudentSystemExeptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (filterContext.Exception != null)
            {
                Log<StudentSystemExeptionAttribute>.Error(filterContext.Exception.Message, filterContext.Exception);
            }
        }
    }
}