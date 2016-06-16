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
    public class PermissionController : BaseController
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
                Value = "",
                Selected = true
            };
            model.roles.Add(defaItem);

            return View(model);
        }
        public ActionResult List(string rid = "")
        {

            var model = _permisService.GetAll(rid);
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
                    if (permissList == null)
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
                            if (!pidList.Contains(f.func.ToString()))
                            {
                                _permisService.DeleteById(f.Id);
                            }
                        }
                        foreach (var f in pidList)
                        {
                            if (!permissList.Where(s => s.func == int.Parse(f)).Any())
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
            return RedirectToAction("List");
        }
    }
}
