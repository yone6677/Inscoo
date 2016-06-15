using Core;
using Core.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using Services;
using Services.Identity;
using Services.Navigation;
using Services.Permission;
using System;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Infrastructure
{
    public class ActionFilter: ActionFilterAttribute
    {
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permisService;
        private readonly INavigationService _navService;
        public ActionFilter(IWebHelper webHelper, IPermissionService permissionService, INavigationService navService)
        {
            _webHelper = webHelper;
            _permisService = permissionService;
            _navService = navService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            var action = filterContext.RouteData.Values["action"].ToString().ToLower();
            if (controller != "account")
            {
                try
                {
                    //var name = filterContext.HttpContext.User.Identity.Name;
                    //var uid = _appUserService.FindByName(name);
                    //var nav = _navService.GetByUrl(controller, action);
                    //bool hasPermission = _permisService.HasPermissionByUser(nav.Id, uid.Id);
                    //if (hasPermission)
                    //{
                    //    base.OnActionExecuting(filterContext);
                    //}
                    //else
                    //{
                    //    filterContext.Result = new RedirectResult("/");
                    //}
                }
                catch (Exception e)
                {
                    throw new HttpException(401, "抱歉,您未被授权查看此页.", e);
                }
            }
            
        }
    }
}