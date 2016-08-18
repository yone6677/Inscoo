using Domain;
using Models.Permission;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Services;
using System.Threading.Tasks;

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
            //GetAllActions("c4c4dee0-a4c2-466c-b705-1fb2c20e27da");
        }
        // GET: Permission
        public ActionResult Index()
        {
            var model = new PermissionModel();
            model.roles = _roleService.GetSelectList();
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
                            var item = new Permission()
                            {
                                NavigationId = int.Parse(f),
                                roleId = rid
                            };
                            _permisService.Insert(item);
                        }
                    }
                    else
                    {
                        foreach (var f in permissList)
                        {
                            if (!pidList.Contains(f.NavigationId.ToString()))
                            {
                                _permisService.DeleteById(f.Id);
                            }
                        }
                        foreach (var f in pidList)
                        {
                            if (!permissList.Where(s => s.NavigationId == int.Parse(f)).Any())
                            {
                                var item = new Permission()
                                {
                                    NavigationId = int.Parse(f),
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

        public ActionResult ManagerSearch()
        {
            try
            {
                var roleList = _roleService.GetRoleSelectList();
                ViewBag.roleId = roleList;
                var controlList = GetAllControles();
                ViewBag.control = controlList;

                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public PartialViewResult ManagerList(string roleId, string control)
        {
            try
            {
                var model = GetAllActions(roleId, control);
                return PartialView(model);
            }
            catch (Exception e)
            {
                return PartialView(new List<vPermissionNav>());
            }

        }
        public JsonResult DeletePermission(string controller, string action, string roleId)
        {
            var result = _permisService.DeleteByUrl(controller, action, roleId);
            return new JsonResult() { Data = new { result = result } };
        }
        public JsonResult AddPermission(string controller, string action, string roleId)
        {
            var result = _permisService.AddByUrl(controller, action, roleId, User.Identity.Name);
            return new JsonResult() { Data = new { result = result } };
        }
        #region private
        List<vPermissionNav> GetAllActions(string roleId, string con)
        {
            try
            {
                var list = new List<vPermissionNav>();

                if (con == "控制器以外")
                {
                    var navs = _navService.GetNotControllerNav();
                    //var tasks = navs.Select(a => Task.Run(() =>
                    //{
                    //    list.Add(new vPermissionNav()
                    //    {

                    //        Controller = "控制器以外",
                    //        Action = a.name,
                    //        IsUsed = _permisService.HasPermissionByRoleId(a.Id, roleId),
                    //        Name = a.name
                    //    });
                    //})
                    //);
                    foreach (var a in navs)
                    {
                        list.Add(new vPermissionNav()
                        {

                            Controller = "控制器以外",
                            Action = a.name,
                            IsUsed = _permisService.HasPermissionByRoleId(a.Id, roleId),
                            Name = a.name
                        });
                    }


                    //Task.WaitAll(tasks.ToArray());
                }
                else
                {
                    var control = typeof(BaseController).Assembly.GetTypes().SingleOrDefault(t => t.Name.Equals(con));

                    var controlName = control.Name;
                    var actions = control.GetMethods().Where(m => m.DeclaringType.Name == control.Name && m.ReturnType.Namespace.Equals("System.Web.Mvc", StringComparison.CurrentCultureIgnoreCase)).ToList();
                    foreach (var a in actions)
                    {
                        var nav = _navService.GetByUrl(controlName, a.Name);
                        var hasAllowAnonymousAttribute =
                            a.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
                        if (hasAllowAnonymousAttribute)
                        {
                            list.Add(new vPermissionNav()
                            {

                                Controller = control.Name,
                                Action = a.Name,
                                IsUsed = true,
                                CanEdit = false,//不允许调整权限
                                Name = nav == null ? a.Name : nav.name
                            });
                        }
                        else
                        {
                            list.Add(new vPermissionNav()
                            {

                                Controller = control.Name,
                                Action = a.Name,
                                IsUsed = _permisService.HasNavUsedByRole(controlName, a.Name, roleId),
                                Name = nav == null ? a.Name : nav.name
                            });
                            //}
                        }

                        /*  var tasks = actions.Select(a => Task.Run(() =>
                          {
                              var nav = _navService.GetByUrl(controlName, a.Name);
                              var hasAllowAnonymousAttribute =
                                  a.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
                              if (hasAllowAnonymousAttribute)
                              {
                                  list.Add(new vPermissionNav()
                                  {

                                      Controller = control.Name,
                                      Action = a.Name,
                                      IsUsed = true,
                                      CanEdit = false,//不允许调整权限
                                      Name = nav == null ? a.Name : nav.name
                                  });
                              }
                              else
                              {
                                  list.Add(new vPermissionNav()
                                  {

                                      Controller = control.Name,
                                      Action = a.Name,
                                      IsUsed = _permisService.HasNavUsedByRole(controlName, a.Name, roleId),
                                      Name = nav == null ? a.Name : nav.name
                                  });
                              }

                          }
                        )
                        );

                          Task.WaitAll(tasks.ToArray());
                          */
                    }
                }
                //foreach (var a in actions)
                //{
                //    var nav = _navService.GetByUrl(controlName, a.Name);
                //    list.Add(new vPermissionNav()
                //    {
                //        Controller = control.Name,
                //        Action = a.Name,
                //        IsUsed = _permisService.HasNavUsedByRole(controlName, a.Name, roleId),
                //        Name = nav == null ? a.Name : nav.name
                //    });
                //}
                return list.OrderBy(l => l.Name).ToList();
            }
            catch (Exception e)
            {
                return new List<vPermissionNav>();
            }

        }
        SelectList GetAllControles()
        {
            var controllers = typeof(BaseController).Assembly.GetTypes().Where(t => t.BaseType.Name.Contains("BaseController")).Select(c => new { Text = c.Name, Value = c.Name }).ToList();
            controllers.Add(new { Text = "控制器以外", Value = "控制器以外" });
            return new SelectList(controllers, "Value", "Text");
        }
        #endregion
    }
}
