using Domain.Permission;
using Microsoft.AspNet.Identity;
using Models.Permission;
using Services.Identity;
using Services.Navigation;
using Services.Permission;
using System;
using System.Linq;
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
        public ActionResult List(string Id = "")
        {
            var model = _permisService.GetAll(Id);
            //var model = new List<PermissionViewModel>();
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save()
        {
            try
            {
                string pids = Request.Form["pids"];
                string rid = Request.Form["Id"];
                if (!string.IsNullOrEmpty(pids))
                {
                    List<string> pidList = new List<string>();
                    if (pids.Contains(","))
                    {
                        string[] tempStr = pids.Split(',');
                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            pidList.Add(tempStr[i].Trim());
                        }
                    }
                    else
                    {
                        pidList.Add(pids);
                    }
                    var permissList = _permisService.GetPermissionByRole(rid);
                    if (!permissList.Any())
                    {
                        foreach (var f in pidList)
                        {
                            var item = new PermissionItem()
                            {
                                func = int.Parse(f),
                                roleId = rid
                            };
                            _permisService.Insert(item);
                        }
                    }
                    else
                    {
                        foreach (var f in permissList)
                        {
                            if (!pidList.Contains(f.Id.ToString()))
                            {
                                _permisService.Delete(f);
                            }
                        }
                        foreach (var f in pidList)
                        {
                            if (!permissList.Where(s => s.Id == int.Parse(f)).Any())
                            {
                                var item = new PermissionItem()
                                {
                                    func = int.Parse(f),
                                    roleId = rid
                                };
                                _permisService.Insert(item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("权限更改失败", e);
            }
            return View();
        }
    }
}
