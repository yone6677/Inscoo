using Domain;
using Inscoo.Models.Account;
using Models.User;
using Microsoft.AspNet.Identity;
using Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using Innscoo.Infrastructure;
using System;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;

namespace Inscoo.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleManager;
        public UserController(IAppUserService appUserService, IAppRoleService appRoleManager)
        {
            _appRoleManager = appRoleManager;
            _appUserService = appUserService;
        }
        // GET: User
        public ActionResult Index()
        {
            var model = new UserListModel();
            model.RoleList = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(UserListModel model)
        {
            var roleList = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");
            if (!string.IsNullOrEmpty(model.RoleId))
                roleList.Find(r => r.Value == model.RoleId).Selected = true;

            model.RoleList = roleList;

            return View(model);
        }
        public ActionResult List(string roleId, string userName)
        {
            //var roleId = Request.QueryString["roleId"];
            //var userName = Request.QueryString["userName"];
            var list = _appUserService.GetUserList(userName: userName, roleId: roleId);
            var command = new PageCommand()
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                TotalPages = list.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(list);
        }
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            var roles = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId());
            var user = _appUserService.FindById(User.Identity.GetUserId());

            ViewBag.maxRebate = user.Rebate;
            //typeof(RegisterModel).GetProperty("Rebate").GetCustomAttributes(false).SetValue(new RangeAttribute(0, user.Rebate) { ErrorMessage = string.Format("不能大于{0}", user.Rebate) }, 1);
            var model = new RegisterModel() { selectList = roles };

            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    BankName = model.BankName,
                    BankNumber = model.BankNumber,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    LinkMan = model.Linkman,
                    CompanyName = model.CompanyName,
                    TiYong = model.TiYong,
                    FanBao = model.FanBao,
                    IsDelete = model.IsDelete,
                    CreaterId = User.Identity.GetUserId()
                };
                var result = await _appUserService.CreateAsync(user, model.UserName, "inscoo");
                if (result.Succeeded)
                {
                    result = await ForRole(user, model.Roles);
                    return View("Details", model);
                }
            }
            return View();
        }

        public Task<IdentityResult> ForRole(AppUser user, string roleName)
        {
            return _appUserService.AddToRoleAsync(user.Id, roleName);
        }
        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            var model = _appUserService.Get_UserModel_ById(id);
            ViewBag.maxRebate = _appUserService.FindById(User.Identity.GetUserId()).Rebate;
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _appUserService.FindById(model.Id);
                    user.CompanyName = model.CompanyName;
                    user.LinkMan = model.LinkMan;
                    user.PhoneNumber = model.Phone;
                    user.Email = model.Email;
                    user.TiYong = model.TiYong.HasValue ? model.TiYong.Value : false;
                    user.FanBao = model.FanBao.HasValue ? model.FanBao.Value : false;
                    user.Rebate = model.Rebate;
                    var result = await _appUserService.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                    {
                        throw new Exception("修改失败");
                    }
                }
                else
                {
                    throw new Exception("输入有误");
                }
            }
            catch
            {
                return View(model);
            }
        }
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    _appUserService.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
