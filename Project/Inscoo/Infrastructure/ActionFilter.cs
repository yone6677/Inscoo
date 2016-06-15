using Services;
using System.Web.Mvc;

namespace Inscoo.Infrastructure
{
    public class ActionFilter: ActionFilterAttribute
    {
        private readonly IWebHelper _webHelper;
        public ActionFilter(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();
        }
    }
}