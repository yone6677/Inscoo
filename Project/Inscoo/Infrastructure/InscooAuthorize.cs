using Microsoft.AspNet.Identity;
using Services;
using System;
using System.Linq;
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
            if (filterContext.HttpContext.User.Identity.Name == "Admin") return;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
            string actionName = filterContext.ActionDescriptor.ActionName;
            var name = filterContext.HttpContext.User.Identity.Name;
            var userId = filterContext.HttpContext.User.Identity.GetUserId();
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()) return;
            if (CommonAuthorizationCheck(controllerName, actionName, userId)) return;

            var httpcontext = filterContext.HttpContext;
            try
            {
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
                throw new HttpException(401, "没有权限", e);
            }
        }

        /// <summary>
        /// 检查通用的权限验证
        /// </summary>
        /// <returns></returns>
        bool CommonAuthorizationCheck(string controllerName, string actionName, string userId)
        {
            var roles = _appUserService.GetRolesByUserId(userId);
            if (roles.Contains("Admin")) return true;
            var onlyIsInscooOperator = roles.Count == 1 && (roles.Contains("InscooOperator"));
            if (onlyIsInscooOperator)
            {
                if (controllerName == "RoleController" || controllerName == "NavController" || controllerName == "PermissionController" || controllerName == "GenericattributeController") return false;
                else
                {
                    return true;
                }

            }
            return false;
        }
    }
}