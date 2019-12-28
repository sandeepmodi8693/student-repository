using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using System.Web.Http.Controllers;

namespace Student.Api.Filters
{
    public class AuthorizationPrivilegeFilter : ActionFilterAttribute
    {
        public AuthorizationPrivilegeFilter()
        { }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{


        //    string userId = HttpContext.Current.User.Identity.GetUserId();
        //    if (userId != null)
        //    {

        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Account" },
        //                                  { "action", "Login" }

        //                                 });

        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            //Trace.WriteLine(string.Format("Action Method {0} executing at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");
        }
    }
}