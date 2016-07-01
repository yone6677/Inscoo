using Domain;
using Models;
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
        private readonly IGenericAttributeService _svGenericAttribute;
        public UserController(IAppUserService appUserService, IAppRoleService appRoleManager, IGenericAttributeService svGenericAttribute)
        {
            _appRoleManager = appRoleManager;
            _appUserService = appUserService;
            _svGenericAttribute = svGenericAttribute;
        }
        // GET: User
        public ActionResult Index()
        {
            ViewBag.RoleId = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");

            return View();
        }

        public ActionResult List(string roleId, string userName)
        {
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
            var roles = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Name");
            var user = _appUserService.FindById(User.Identity.GetUserId());
            ViewBag.maxRebate = user.Rebate;
            var model = new RegisterModel() { RoleSelects = roles, CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "") };

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
                    CreaterId = User.Identity.GetUserId(),
                    CommissionMethod = model.CommissionMethod,
                    AccountName = model.AccountName
                };
                var result = await _appUserService.CreateAsync(user, model.UserName, "inscoo");
                if (result.Succeeded)
                {
                    if (ForRole(user, model.Roles))
                        return RedirectToAction("Index");
                    else
                    {
                        return View();
                    }
                }
            }
            return View();
        }

        public bool ForRole(AppUser user, string roleName)
        {
            return _appUserService.DeleteBeforeRoleAndNew(user.Id, roleName);
        }
        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            var model = _appUserService.Get_RegisterModel_ById(id);
            model.CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", model.CommissionMethod);

            var roles = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Name", model.Roles);

            model.RoleSelects = roles;
            ViewBag.maxRebate = _appUserService.FindById(User.Identity.GetUserId()).Rebate;
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _appUserService.FindById(model.Id);
                    user.UserName = model.UserName;
                    user.CompanyName = model.CompanyName;
                    user.LinkMan = model.Linkman;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.TiYong = model.TiYong;
                    user.FanBao = model.FanBao;
                    user.Rebate = model.Rebate;
                    user.BankName = model.BankName;
                    user.BankNumber = model.BankNumber;
                    user.CommissionMethod = model.CommissionMethod;
                    user.AccountName = model.AccountName;
                    user.IsDelete = model.IsDelete;

                    var result = await _appUserService.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (ForRole(user, model.Roles))
                            return RedirectToAction("Index");
                        else
                        {
                            return View();
                        }
                        //return View("Details", model);
                    }
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
