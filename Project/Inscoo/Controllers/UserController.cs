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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Pager;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Configuration;
using System.ComponentModel;

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
        #region User
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
            var uId = User.Identity.GetUserId();
            var isAdmin = _appUserService.GetRolesByUserId(uId).Contains("Admin");
            var roles = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Name");
            var user = _appUserService.FindById(User.Identity.GetUserId());
            ViewBag.maxRebate = user.Rebate;
            var model = new RegisterModel() { RoleSelects = roles, };
            if (!isAdmin)
            {
                if (user.CommissionMethod != "Nothing")
                {
                    model.CommissionMethods = null;
                    ViewBag.CommissionMethod = _svGenericAttribute.GetByKey(value: user.CommissionMethod).Key;
                }
                else
                {
                    model.CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "");
                }
            }
            else
            {
                model.CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "");
            }

            ViewBag.ProdSeriesList = _appUserService.GetProdSeries(uId);
            ViewBag.ProdInsurancesList = _appUserService.GetProdInsurances(uId);
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

                if (model.CommissionMethod == null)
                {
                    var u = _appUserService.FindById(User.Identity.GetUserId());
                    model.CommissionMethod = _svGenericAttribute.GetByKey(value: u.CommissionMethod).Value;
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
                    ProdInsurance = ProdInsurance,
                    Memo = model.Memo

                };
                var defaultPwd = ConfigurationManager.AppSettings["newPwd"];
                var result = await _appUserService.CreateAsync(user, model.UserName, defaultPwd);
                if (result.Succeeded)
                {
                    if (ForRole(user, model.Roles))
                    {
                        var mailContent = string.Format("<p><b>{0}</b>,您好：</p><div style=\"text-indent:4em;\"><p>已为您开通保酷平台的用户权限，请登录使用，详情如下：</p><p>            登录网站：<b>www.inscoo.com</b></p><p>            登录账号：<b>{1}</b></p><p>            密码：<b>inscoo</b></p><p>请您在首次登录后立即修改密码，谢谢！</p><br><p>如果有任何疑问，请随时拨打400-612-6750咨询！</p><p>欢迎加入保酷大家庭，祝您工作愉快，顺祝商祺！</p><br></div><p><b>保酷网 www.inscoo.com</b></p><p style=\"overflow:hidden\"><img src=\"http://www.inscoo.com/Content/img/InscooLogo.png\"alt=\"\"style=\"float: left;\" /><img src=\"http://www.inscoo.com/Content/img/InscooWeChat.png\" alt=\"\" style=\"float: left;\" /></p><p>上海皓为商务咨询有限公司</p>", user.UserName, user.Email);
                        MailService.SendMail(new MailQueue()
                        {
                            MQTYPE = "保酷账号",
                            MQSUBJECT = "保酷账号",
                            MQMAILCONTENT = mailContent,
                            MQMAILFRM = "service@inscoo.com",
                            MQMAILTO = user.Email,
                            //MQFILE = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\caozuozhinan.docx"

                        });
                        return RedirectToAction("Index", new { successMes = "添加成功" });
                    }
                }
            }
            return RedirectToAction("Index", new { errorMes = "添加失败" });
        }
        #endregion

        #region CreateAccountCode
        public ActionResult TIndex(string erorrMes, string successMes)
        {
            ViewBag.RoleId = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");
            var roles = _appUserService.GetRolesByUserId(User.Identity.GetUserId());
            ViewBag.CanCreate = !(roles.Contains("InsuranceCompany") && roles.Count == 1);

            ViewData["ErorrMes"] = erorrMes;
            ViewData["SuccessMes"] = successMes;

            return View();
        }

        public ActionResult TList(string roleId, string company, int pageIndex = 1, int pageSize = 15)
        {
            var uId = User.Identity.GetUserId();

            //出admin外，其他用户只能看到自己创建的用户
            var list = _appUserService.GetCreateAccountList(pageIndex, pageSize, company, roleId);


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
        public ActionResult TCreate(int id = -1, string type = "Create")
        {
            var model = new TRegisterModel();

            if (id != -1)
            {
                model = _appUserService.GetTRegisterModelById(id);
            }
            var uId = User.Identity.GetUserId();
            var roles = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Name", model.Roles);
            var user = _appUserService.FindById(User.Identity.GetUserId());
            ViewBag.maxRebate = user.Rebate;
            model.RoleSelects = roles;


            var CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "");
            var CommissionMethodsSelected = CommissionMethods.FirstOrDefault(i => i.Value == model.CommissionMethod);
            if (CommissionMethodsSelected != null)
            {
                CommissionMethodsSelected.Selected = true;
            }

            model.CommissionMethods = CommissionMethods;
            ViewBag.ProdSeriesList = _appUserService.GetProdSeries(uId);
            ViewBag.ProdInsurancesList = _appUserService.GetProdInsurances(uId);
            model.Type = type;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TCreate(TRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EncryBeginDate > model.EncryEndDate)
                {
                    return RedirectToAction("TIndex", new { errorMes = "日期不正确" });
                }
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

                //if (model.CommissionMethod != null)
                //{
                //    var u = _appUserService.FindById(User.Identity.GetUserId());
                //    model.CommissionMethod = _svGenericAttribute.GetByKey(value: u.CommissionMethod).Value;
                //}
                bool result = false;
                if (model.Type == "Create")
                {
                    #region create
                    var user = new CreateAccountCode()
                    {
                        AccountEncryCode = GetAccountEncryCode(),
                        EncryBeginDate = model.EncryBeginDate,
                        EncryEndDate = model.EncryEndDate,
                        EncryCompanyName = model.CompanyName,
                        EncryTiYong = model.TiYong,
                        EncryFanBao = model.FanBao,
                        Author = User.Identity.Name,
                        EncryCommissionMethod = model.CommissionMethod,
                        EncryRebate = model.Rebate,
                        EncrySeries = ProdSeries,
                        EncryInsurance = ProdInsurance,
                        EncryMemo = model.Memo,
                        EncryCreateID = User.Identity.GetUserId(),
                        EncryRoleName = model.Roles
                    };
                    result = _appUserService.AddCreateAccountCode(user);
                    #endregion
                }
                else if (model.Type == "Edit")
                {
                    var user = _appUserService.GetCreateAccountCode(model.Id);
                    //user.AccountEncryCode = GetAccountEncryCode();
                    user.EncryBeginDate = model.EncryBeginDate;
                    user.EncryEndDate = model.EncryEndDate;
                    user.EncryCompanyName = model.CompanyName;
                    user.EncryTiYong = model.TiYong;
                    user.EncryFanBao = model.FanBao;
                    //user.Author = User.Identity.Name;
                    user.EncryCommissionMethod = model.CommissionMethod;
                    user.EncryRebate = model.Rebate;
                    user.EncrySeries = ProdSeries;
                    user.EncryInsurance = ProdInsurance;
                    user.EncryMemo = model.Memo;
                    user.EncryCreateID = User.Identity.GetUserId();
                    //user.EncryRoleName = model.Roles;

                    result = _appUserService.UpdateAccountCode(user);
                }
                else if (model.Type == "Delete")
                {
                    result = _appUserService.DeleteAccountCode(model.Id);
                }
                if (result)
                {
                    return RedirectToAction("TIndex", new { successMes = "操作成功" });
                }
            }
            return RedirectToAction("TIndex", new { errorMes = "操作失败" });
        }
        [AllowAnonymous]
        public ActionResult EncryInfo(string mes)
        {
            var model = new EncryInfoModel();
            model.Mes = mes;
            return PartialView(model);
        }

        // POST: EncryInfo/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EncryInfo(EncryInfoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["IdentifyCode"].ToString() != Request.Form["IdentifyCode"])
                    {
                        model.Mes = "验证码不正确";
                        return View(model);
                    }
                    model.Password = ConfigurationManager.AppSettings["newPwd"];
                    var result = await _appUserService.CreateUserByEncry(model);
                    if (result)
                    {
                        model.Mes = "激活成功";
                        model = new EncryInfoModel();
                        return RedirectToAction("EncryInfo", new { mes = "激活成功" });
                    }
                    else
                    {
                        model.Mes = "激活失败";
                    }
                }
            }
            catch (WarningException e)
            {
                model.Mes = e.Message;
            }
            catch (Exception e)
            {
                model.Mes = "激活失败";
            }

            return View(model);
        }
        #endregion
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
            var uId = User.Identity.GetUserId();
            var isAdmin = _appUserService.GetRolesByUserId(uId).Contains("Admin");
            var model = _appUserService.Get_RegisterModel_ById(id);

            if (!isAdmin)
            {
                var user = _appUserService.FindById(User.Identity.GetUserId());
                if (user.CommissionMethod != "Nothing")
                {
                    model.CommissionMethods = null;
                    ViewBag.CommissionMethod = _svGenericAttribute.GetByKey(value: user.CommissionMethod).Key;
                }
                else
                {
                    model.CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "");
                }
            }
            else
            {
                model.CommissionMethods = _svGenericAttribute.GetSelectListByGroup("CommissionMethod", "");
            }


            var roles = _appUserService.GetRolesManagerPermissionByUserId(uId, "Name", model.Roles);

            model.RoleSelects = roles;
            ViewBag.ProdSeriesList = _appUserService.GetProdSeries(uId);
            ViewBag.ProdInsurancesList = _appUserService.GetProdInsurances(uId);
            ViewBag.maxRebate = _appUserService.FindById(User.Identity.GetUserId()).Rebate;
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    if (model.CommissionMethod == null)
                    {
                        var u = _appUserService.FindById(User.Identity.GetUserId());
                        model.CommissionMethod = _svGenericAttribute.GetByKey(value: u.CommissionMethod).Value;
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
                    user.Memo = model.Memo;
                    if (_appUserService.Update(user))
                    {
                        if (ForRole(user, model.Roles))
                            return RedirectToAction("Index", new { successMes = "修改成功" });
                        else
                        {
                            throw new Exception("角色创建失败");
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

        #region private
        string GetAccountEncryCode()
        {
            return "";
        }
        #endregion
    }
}
