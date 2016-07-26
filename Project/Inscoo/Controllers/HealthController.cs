using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Core;
using Innscoo.Infrastructure;
using Microsoft.AspNet.Identity;
using Models;
using Models.Infrastructure;
using Models.Order;
using Services;

namespace Inscoo.Controllers
{
    public class HealthController : BaseController
    {
        private readonly IHealthService _svHealth;
        private readonly ICompanyService _svCompany;
        private readonly AppUserManager _svAppUserManager;
        private readonly IGenericAttributeService _genericAttributeService;

        public HealthController(IGenericAttributeService genericAttributeService, AppUserManager svAppUserManager, ICompanyService svCompany, IHealthService svHealth)
        {
            _genericAttributeService = genericAttributeService;
            _svAppUserManager = svAppUserManager;
            _svHealth = svHealth;
            _svCompany = svCompany;
        }

        // GET: Health
        public ActionResult Index()
        {
            var model = _svHealth.GetHealthProducts(User.Identity.GetUserId());
            ViewBag.ProductTypeName = model.Select(m => m.ProductTypeName).Distinct().ToList();
            return View(model);
        }

        /// <summary>
        /// 产品信息页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BuyInfo(int id)
        {
            var model = _svHealth.GetHealthProductById(id, User.Identity.GetUserId());
            return View(model);
        }

        /// <summary>
        /// 购买流程-方案确认
        /// </summary>
        /// <param name="productId"></param>
        ///  /// <param name="masterId"></param>
        /// <returns></returns>
        public ActionResult MakeSure(int productId = -1, int masterId = -1)
        {

            if (masterId != -1)//返回逻辑
            {
                productId = _svHealth.GetHealthMaster(masterId).HealthCheckProductId;
            }
            var model = _svHealth.GetHealthProductById(productId, User.Identity.GetUserId());
            model.MasterId = masterId;
            return View(model);
        }

        /// <summary>
        /// 购买流程-信息填写
        /// </summary>
        /// <param name="productId"></param>
        ///   /// <param name="masterId"></param>
        /// <returns></returns>
        public ActionResult EntryInfo(int productId = -1, int masterId = -1)
        {
            try
            {

                if (masterId == -1)
                {
                    var model = new VHealthEntryInfo();
                    model.MasterId = _svHealth.AddHealthMaster(productId, User.Identity.Name);
                    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                    return View(model);
                }
                else
                {
                    var model = _svHealth.GetHealthEntryInfo(masterId, User.Identity.Name);
                    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId(), model.CompanyId);
                    return View(model);
                }

            }
            catch (Exception)
            {
                return RedirectToAction("MakeSure", new { productId, masterId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntryInfo(VHealthEntryInfo model)
        {

            if (ModelState.IsValid)
            {
                var master = _svHealth.GetHealthMaster(model.MasterId, User.Identity.Name);
                var companyList = Request.Form["isCompanySelect"];
                if (companyList == "false")
                {
                    master.CompanyId =
                        _svCompany.AddNewCompany(
                            new vCompanyAdd()
                            {
                                Name = model.CompanyName,
                                Address = model.Address,
                                Email = "",
                                LinkMan = model.Linkman,
                                Phone = model.PhoneNumber
                            }, User.Identity.GetUserId());
                }
                else
                {
                    master.CompanyId = Convert.ToInt32(Request.Form["CompanyId"]);
                }
                master.Status = 4;
                _svHealth.UpdateMaster(master);
            }
            return RedirectToAction("ConfirmPayment", new { model.MasterId });
        }

        /// <summary>
        /// OP  审核订单
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AuditOrder(int masterId, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var model = _svHealth.GetHealthAuditOrder(masterId);
                model.PageIndex = pageIndex;
                model.PageSize = pageSize;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("AuditListSearch", new { masterId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditOrder(VHealthAuditOrder model)
        {

            if (ModelState.IsValid)
            {
                var master = _svHealth.GetHealthMaster(model.MasterId);
                var result = Request.Form["result"] == "1";
                master.Status = result ? 11 : 14;
                master.BaokuConfirmDate = DateTime.Now;
                master.BaokuConfirmer = User.Identity.Name;
                _svHealth.UpdateMaster(master);
            }
            return RedirectToAction("AuditListSearch", new { model.PageIndex, model.PageSize });
        }

        //财务确认收款
        public ActionResult FnComfirm(int masterId, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var model = new VFNConfirm() { MasterId = masterId, PageIndex = pageIndex, PageSize = pageSize, FinancePayDate = DateTime.Now };

                return View(model);

            }
            catch (Exception)
            {
                return RedirectToAction("AuditListSearch", new { masterId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FnComfirm(VFNConfirm model)
        {

            if (ModelState.IsValid)
            {
                var master = _svHealth.GetHealthMaster(model.MasterId);

                master.FinanceAmount = model.FinanceAmount;
                master.FinanceBankSerialNumber = model.FinanceBankSerialNumber;
                master.FinanceMemo = model.FinanceMemo;
                master.FinancePayDate = model.FinancePayDate;
                master.FinanceConfirmDate = DateTime.Now;
                master.FinanceConfirmer = User.Identity.Name;
                master.Status = 17;
                master.BaokuOrderCode = "HLTH" + string.Format("{0:0000000000}", master.Id);
                _svHealth.UpdateMaster(master);

                var mailContent = $"体检订单：{ master.BaokuOrderCode}已确认付款";
                var mailTo = _genericAttributeService.GetByGroup("HealthFinanceMailTo").Select(c => c.Value);
                MailService.SendMailAsync(new MailQueue()
                {
                    MQTYPE = "HealthOrder",
                    MQSUBJECT = "体检订单确认付款通知",
                    MQMAILCONTENT = "",
                    MQMAILFRM = "redy.yone@inscoo.com",
                    MQMAILTO = string.Join(";", mailTo),
                    MQFILE = ""

                });

            }
            return RedirectToAction("AuditListSearch", new { model.PageIndex, model.PageSize });
        }
        /// <summary>
        /// 上传人员列表
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">15</param>
        /// <returns></returns>
        public ActionResult HealthPersons(int masterId, int pageIndex = 1, int pageSize = 15)
        {
            if (masterId > 0)
            {
                var model = _svHealth.GetHealthOrderDetails(pageIndex, pageSize, masterId);
                var command = new PageCommand()
                {
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    TotalCount = model.TotalCount,
                    TotalPages = model.TotalPages
                };
                ViewBag.pageCommand = command;
                return PartialView(model);
            }
            return null;
        }
        /// <summary>
        /// 上传名单
        /// </summary>
        /// <param name="empinfo"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadEmp(HttpPostedFileBase empinfo, int masterId)
        {
            try
            {
                _svHealth.UploadEmpExcel(empinfo, masterId, User.Identity.Name);
                //todo 付款通知书
                _svHealth.GetPaymentNoticePdfAsync(masterId);
            }
            catch (WarningException e)
            {
                TempData["error"] = e.Message;

            }
            catch (Exception e)
            {
                TempData["error"] = "上传失败！";

            }
            return RedirectToAction("EntryInfo", new { masterId });
        }
        /// <summary>
        /// 确认付款
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public ActionResult ConfirmPayment(int masterId)
        {
            if (masterId > 0)
            {
                var model = _svHealth.GetConfirmPayment(masterId);
                if (model != null)
                {
                    if (string.IsNullOrEmpty(model.PaymentNoticePdf))//还未产生付款通知书
                    {
                        model.PaymentNoticePdf = _svHealth.GetPaymentNoticePdf(masterId);
                    }
                    return View(model);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditListSearch(int pageIndex = 1, int pageSize = 15)
        {
            var model = new VHealthSearch();
            model.ListTypeList = _svHealth.GetListType(User.Identity.GetUserId());// 1客户未完成，2客户已完成，未审核，3已审核,4客户已完成
            model.IsInscooOperator = _svAppUserManager.GetRoles(User.Identity.GetUserId()).Contains("InscooOperator");
            model.IsFinance = _svAppUserManager.GetRoles(User.Identity.GetUserId()).Contains("InscooFinance");
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;

            return View(model);
        }
        public PartialViewResult AuditListData(bool isInscooOperator, bool isFinance, int listType, int pageIndex = 1, int pageSize = 15)
        {
            //var isInscooOperator = _svAppUserManager.GetRoles(User.Identity.GetUserId()).Contains("InscooOperator");
            ViewBag.IsInscooOperator = isInscooOperator;
            ViewBag.isFinance = isFinance;
            var search = new VHealthSearch() { ListType = listType, IsFinance = isFinance, IsInscooOperator = isInscooOperator };
            //if (isInscooOperator) search.ListType = 4;
            var list = _svHealth.GetHealthAuditList(pageIndex, pageSize, User.Identity.Name, search);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult AuditListData(VHealthSearch model, int pageIndex = 1, int pageSize = 15)
        {
            ViewBag.IsInscooOperator = model.IsInscooOperator;
            ViewBag.IsFinance = model.IsFinance;
            var list = _svHealth.GetHealthAuditList(pageIndex, pageSize, User.Identity.Name, model);
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
    }
}