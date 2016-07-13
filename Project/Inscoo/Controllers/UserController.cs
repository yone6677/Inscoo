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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Pager;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Inscoo.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleManager;
        private readonly IGenericAttributeService _svGenericAttribute;
        private readonly IArchiveService _archiveService;
        private readonly IPermissionService _svPermissionService;
        public UserController(IAppUserService appUserService, IAppRoleService appRoleManager, IGenericAttributeService svGenericAttribute, IArchiveService archiveService, IPermissionService svPermissionService)
        {
            _appRoleManager = appRoleManager;
            _appUserService = appUserService;
            _svGenericAttribute = svGenericAttribute;
            _archiveService = archiveService;
            _svPermissionService = svPermissionService;

        }
        // GET: User
        public ActionResult Index(string erorrMes, string successMes)
        {
            ViewBag.RoleId = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");
            var roles = _appUserService.GetRolesByUserId(User.Identity.GetUserId());
            ViewBag.CanCreate = !(roles.Contains("InsuranceCompany") && roles.Count == 1);

            ViewData["ErorrMes"] = erorrMes;
            ViewData["SuccessMes"] = successMes;

            return View();
        }

        public ActionResult List(string roleId, string userName, int pageIndex = 1, int pageSize = 15)
        {
            var uId = User.Identity.GetUserId();

            //出admin外，其他用户只能看到自己创建的用户
            IPagedList<UserModel> list;
            if (_appUserService.GetRolesByUserId(uId).Contains("Admin"))
                list = _appUserService.GetUserList(userName: userName, roleId: roleId, pageIndex: pageIndex, pageSize: pageSize);
            else
                list = _appUserService.GetUserList(userName: userName, roleId: roleId, createUserId: uId, pageIndex: pageIndex, pageSize: pageSize);


            var command = new PageCommand()
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                TotalPages = list.TotalPages
            };
            ViewBag.pageCommand = command;
            ViewBag.CanEdit = _svPermissionService.HasPermissionByUser(71, User.Identity.GetUserId());
            //ViewBag.CanDelete = _svPermissionService.HasPermissionByUser(72, User.Identity.GetUserId());
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

            ViewBag.ProdSeriesList = _svGenericAttribute.GetSelectList("ProductSeries");
            ViewBag.ProdInsurancesList = _svGenericAttribute.GetSelectList("InsuranceCompany");
            //model.ProdSeries = user.ProdSeries.Split(';');
            //model.ProdInsurances = user.ProdInsurance.Split(';');

            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                var uId = User.Identity.GetUserId();
                var ProdSeries = "";
                if (model.ProdSeries != null)
                {
                    foreach (var item in model.ProdSeries)
                    {
                        ProdSeries += item + ';';
                    }
                }
                var ProdInsurance = "";
                if (model.ProdInsurances != null)
                {
                    foreach (var item in model.ProdInsurances)
                    {
                        ProdInsurance += item + ';';
                    }
                }
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
                    CreaterId = uId,
                    Changer = uId,
                    CommissionMethod = model.CommissionMethod,
                    AccountName = model.AccountName,
                    Rebate = model.Rebate,
                    ProdSeries = ProdSeries,
                    ProdInsurance = ProdInsurance

                };
                var result = await _appUserService.CreateAsync(user, model.UserName, "inscoo");
                if (result.Succeeded)
                {
                    if (ForRole(user, model.Roles))
                    {
                        return RedirectToAction("Index", new { successMes = "添加成功" });
                    }
                }
            }
            return RedirectToAction("Index", new { errorMes = "添加失败" });
        }

        [AllowAnonymous]
        public JsonResult IsUserExist(string email)
        {
            var isExist = false;
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    isExist = _appUserService.IsUserExist(userName);
            //}
            //if (isExist) return Json("用户名已使用", JsonRequestBehavior.AllowGet);

            if (!string.IsNullOrEmpty(email))
            {
                isExist = _appUserService.IsUserExist(email);
            }
            if (isExist) return Json("邮箱已使用", JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
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
            ViewBag.ProdSeriesList = _svGenericAttribute.GetSelectList("ProductSeries");
            ViewBag.ProdInsurancesList = _svGenericAttribute.GetSelectList("InsuranceCompany");
            ViewBag.maxRebate = _appUserService.FindById(User.Identity.GetUserId()).Rebate;
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]

        public ActionResult Edit(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UserName = model.Email;
                    var ProdSeries = "";
                    if (model.ProdSeries != null)
                    {
                        foreach (var item in model.ProdSeries)
                        {
                            ProdSeries += item + ';';
                        }
                    }
                    var ProdInsurance = "";
                    if (model.ProdInsurances != null)
                    {
                        foreach (var item in model.ProdInsurances)
                        {
                            ProdInsurance += item + ';';
                        }
                    }

                    var user = _appUserService.FindById(model.Id);
                    user.UserName = model.UserName;
                    user.CompanyName = model.CompanyName;
                    user.LinkMan = model.Linkman;
                    user.PhoneNumber = model.PhoneNumber;
                    //user.Email = model.Email;
                    user.TiYong = model.TiYong;
                    user.FanBao = model.FanBao;
                    user.Rebate = model.Rebate;
                    user.BankName = model.BankName;
                    user.BankNumber = model.BankNumber;
                    user.CommissionMethod = model.CommissionMethod;
                    user.AccountName = model.AccountName;
                    user.IsDelete = model.IsDelete;
                    user.Changer = User.Identity.GetUserId();
                    user.ProdSeries = ProdSeries;
                    user.ProdInsurance = ProdInsurance;
                    if (_appUserService.Update(user))
                    {
                        if (ForRole(user, model.Roles))
                            return RedirectToAction("Index", new { successMes = "修改成功" });
                        else
                        {
                            throw new Exception();
                        }
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
            catch (Exception e)
            {
                return RedirectToAction("Index", new { errorMes = e.Message });
            }
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult ChangePortrait()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePortrait(HttpPostedFileBase portrait)
        {
            try
            {
                var user = _appUserService.GetCurrentUser();
                var path = _archiveService.InsertUserPortrait(portrait);
                user.PortraitPath = path;
                _appUserService.UpdateAsync(user);
                Request.Cookies.Set(new HttpCookie("PortraitPath", path) { HttpOnly = true, Expires = DateTime.Now.AddYears(1) });
                ViewBag.SuccessMes = "修改成功";

            }
            catch (Exception)
            {
                ViewBag.ErrorMes = "修改失败";
            }
            return View();
        }

    }
}
