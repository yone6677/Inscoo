using Microsoft.AspNet.Identity;
using Models.Permission;
using Services.Identity;
using Services.Navigation;
using Services.Permission;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permisService;
        private readonly INavigationService _navService;
        private readonly IAppRoleService _roleService;
        public PermissionController(IPermissionService permissionService, INavigationService navigationService, IAppRoleService appRoleService)
        {
            _permisService = permissionService;
            _navService = navigationService;
            _roleService = appRoleService;
        }
        // GET: Permission
        public ActionResult Index()
        {
            var model = new PermissionViewModel();
            model.roles = _roleService.GetSelectList();
            var defaItem = new SelectListItem()
            {
                Text = "请选择",
                Value = "0",
                Selected = true
            };
            return View(model);
        }
        public ActionResult List(string rid = "")
        {
            var model = _permisService.GetAll(rid);
            //var model = new List<PermissionViewModel>();
            return PartialView(model);
        }
    }
}
