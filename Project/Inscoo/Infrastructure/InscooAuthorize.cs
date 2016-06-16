using Services.Identity;
using Services.Navigation;
using Services.Permission;
using System;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Infrastructure
{
    public class InscooAuthorize : AuthorizeAttribute
    {
        private readonly IPermissionService _permisService;
        private readonly INavigationService _navService;
        private readonly IAppUserService _appUserService;
        public InscooAuthorize()
        {
            _permisService = DependencyResolver.Current.GetService<IPermissionService>();
            _navService = DependencyResolver.Current.GetService<INavigationService>(); ;
            _appUserService = DependencyResolver.Current.GetService<IAppUserService>(); ;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            var httpcontext = filterContext.HttpContext;
            try
            {
                var name = httpcontext.User.Identity.Name;
                if (string.IsNullOrEmpty(name))
                {
                    filterContext.Result = new RedirectResult("/account/login");
                }
                var uid = _appUserService.FindByName(name);
                var nav = _navService.GetByUrl(controllerName, actionName);
                if (nav != null)
                {
                    bool hasPermission = _permisService.HasPermissionByUser(nav.Id, uid.Id);
                    if (hasPermission)
                    {
                        base.OnAuthorization(filterContext);
                    }
                    else
                    {
                        throw new HttpException(401, "抱歉，您未被授权查看此页面");
                    }
                }
                else
                {
                    throw new HttpException(401, "抱歉，您未被授权查看此页面");
                }
            }
            catch (Exception e)
            {
                throw new HttpException(401, "抱歉，您未被授权查看此页面",e);
            }
        }
    }
}