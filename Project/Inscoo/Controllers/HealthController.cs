using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Innscoo.Infrastructure;
using Microsoft.AspNet.Identity;
using Models;
using Models.Infrastructure;
using Models.Order;
using Services;

namespace Inscoo.Controllers
{
    public class HealthController : Controller
    {
        private readonly IHealthService _svHealth;
        private readonly ICompanyService _svCompany;

        public HealthController(ICompanyService svCompany, IHealthService svHealth)
        {
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
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;

            }
            return RedirectToAction("EntryInfo", new { masterId });
        }
        /// <summary>
        /// 取人付款
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
                        //var ls = _orderEmpService.GetPaymentNoticePdf(id);
                        //var fid = _archiveService.InsertByUrl(ls, FileType.PaymentNotice.ToString(), id, "付款通知书");
                        //orderBatch.PaymentNoticePDF = fid;
                        //if (_orderBatchService.Update(orderBatch))
                        //{
                        //    model.PaymentNoticeUrl = ls[1];
                        //    order.State = 4;//付款通知书已下载
                        //    _orderService.Update(order);
                        //}
                    }
                    else
                    {
                        //model.PaymentNoticeUrl = _archiveService.GetById(orderBatch.PaymentNoticePDF).Url;
                    }
                    //model.YearPrice = order.AnnualExpense;
                    //model.MonthPrice = double.Parse((order.AnnualExpense / 12).ToString());
                    //model.Quantity = order.InsuranceNumber;
                    //model.Amount = order.AnnualExpense * order.InsuranceNumber;
                    return View(model);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}