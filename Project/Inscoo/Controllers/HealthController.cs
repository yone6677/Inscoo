using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using Innscoo.Infrastructure;
using Microsoft.AspNet.Identity;
using Models;
using Models.Infrastructure;
using Models.Order;
using Services;
using Newtonsoft.Json;
using Models.Cart;

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
        /// <param name="productName"></param>
        /// <param name="productType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BuyInfo(string productName, string productType, int id = -1)
        {
            var products = _svHealth.GetHealthProducts(User.Identity.GetUserId(), productType, productName);
            VCheckProductList targetP = id == -1 ? products.First() : products.First(p => p.Id == id);
            //var model = new VCheckProductDetail() { Id = targetP.Id, CheckProductPic = targetP.CheckProductPic, PrivilegePrice = targetP.PrivilegePrice, PublicPrice = targetP.PublicPrice, ProductName = targetP.ProductName };

            //var companys = new Dictionary<int, string>();
            //products.ForEach(p => companys.Add(p.Id, p.CompanyName));
            //ViewBag.Companys = companys;
            ViewBag.Product = new VCheckProductDetail() { Id = targetP.Id, CheckProductPic = targetP.CheckProductPic, PrivilegePrice = targetP.PrivilegePrice, PublicPrice = targetP.PublicPrice, ProductName = targetP.ProductName };
            return View(products);
        }
        [AllowAnonymous]
        public PartialViewResult BuyDetail(int productId, string productName, string productType)
        {
            var buyDetail = new VBuyDetail() { ProductId = productId, ProductName = productName, ProductType = productType };
            return PartialView(buyDetail);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyDetail(VBuyDetail model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("MakeSure", new { model.ProductId, model.Count });
            }
            return RedirectToAction("BuyInfo", new { model.ProductName, model.ProductType });
        }
        /// <summary>
        /// 购买流程-方案确认
        /// </summary>
        /// <param name="productId"></param>
        ///  /// <param name="masterId"></param>
        /// <returns></returns>
        public ActionResult MakeSure(int count = -1, int productId = -1, int masterId = -1, string dateTicks = "-1")
        {
            try
            {
                var cartTmpData = TempData["cartTmpData"];
                var model = new List<VCheckProductDetail>();
                if (cartTmpData == null)
                {
                    cartTmpData = "[{\"Id\":" + productId + ",\"Count\":" + count + "}]";
                    var item = new VCheckProductDetail();
                    Domain.HealthOrderMaster master = new Domain.HealthOrderMaster();
                    if (masterId != -1)//返回逻辑
                    {
                        master = _svHealth.GetHealthMaster(masterId, dateTicks: dateTicks);
                        productId = master.HealthCheckProductId;
                        item.MasterId = masterId;
                        item.DateTicks = dateTicks;
                        item.Count = master.Count;
                    }
                    item = _svHealth.GetHealthProductById(productId, User.Identity.GetUserId());
                    item.Count = count;
                    item.SubTotal = item.Count * item.PrivilegePrice;
                    model.Add(item);
                }
                else
                {
                    var cartList = JsonConvert.DeserializeObject<List<CartBuyModel>>(cartTmpData.ToString());
                    if (cartList.Count > 0)
                    {
                        foreach (var c in cartList)
                        {
                            var item = new VCheckProductDetail();
                            var master = new Domain.HealthOrderMaster();
                            //if (masterId != -1)//返回逻辑
                            //{
                            //    master = _svHealth.GetHealthMaster(masterId, dateTicks: dateTicks);
                            //    productId = master.HealthCheckProductId;
                            //}
                            item = _svHealth.GetHealthProductById(c.Id, User.Identity.GetUserId());
                            item.MasterId = c.Id;
                            item.DateTicks = dateTicks;
                            item.Count = c.Count;
                            item.SubTotal = item.Count * item.PrivilegePrice;
                            model.Add(item);
                        }
                    }
                }
                ViewBag.cartTmpData = cartTmpData;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("index");
            }
        }

        public ActionResult EntryInfo(string prodStr = null, string ticks = null)
        {
            try
            {
                var model = new VHealthEntryInfo();
                if (!string.IsNullOrEmpty(prodStr))
                {
                    var cartList = JsonConvert.DeserializeObject<List<CartBuyModel>>(prodStr);
                    var healthList = _svHealth.AddHealthMaster(cartList, User.Identity.Name);

                    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                    model.DateTicks = healthList.FirstOrDefault().DateTicks;
                }
                if (!string.IsNullOrEmpty(ticks))
                {
                    var healthList = _svHealth.GetByTicks(ticks, User.Identity.Name);
                    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                    model.DateTicks = ticks;
                }
                return View(model);
                //if (masterId == -1)
                //{
                //    var model = new VHealthEntryInfo();
                //    var master = _svHealth.AddHealthMaster(productId, User.Identity.Name, count);
                //    model.MasterId = master.Id;
                //    model.DateTicks = master.DateTicks;
                //    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                //    return View(model);
                //}
                //else
                //{
                //    var model = _svHealth.GetHealthEntryInfo(masterId, User.Identity.Name);
                //    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId(), model.CompanyId);
                //    return View(model);
                //}

            }
            catch (Exception)
            {
                TempData["cartTmpData"] = prodStr;
                return RedirectToAction("MakeSure");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntryInfo(VHealthEntryInfo model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.DateTicks))
                    {
                        var healthList = _svHealth.GetByTicks(model.DateTicks, User.Identity.Name);
                        foreach (var healthItem in healthList)
                        {
                            var master = _svHealth.GetHealthMaster(healthItem.Id, author: User.Identity.Name);
                            var IsGetPaymentNoticePdf = !master.CompanyId.HasValue;
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

                            if (IsGetPaymentNoticePdf)
                            {
                                _svHealth.GetPaymentNoticePdfAsync(model.DateTicks);
                            }
                            _svHealth.UpdateMaster(master);
                        }
                    }

                }
                return RedirectToAction("ConfirmPayment", new { model.DateTicks });

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// OP  审核订单
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isLookInfo">是否是查看订单信息</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult OrderInfo(int masterId, string dateTicks, bool isInline = false, bool isDelete = false, int pageIndex = 1, int pageSize = 15, string mes = "")
        {
            try
            {
                var model = _svHealth.GetHealthAuditOrder(masterId, dateTicks);
                model.PageSize = pageSize;
                model.PageIndex = pageIndex;
                ViewBag.IsInline = isInline;
                ViewBag.IsDelete = isDelete;
                ViewBag.Mes = mes;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("AuditListSearch", new { masterId });
            }
        }
        [AllowAnonymous]
        public ActionResult Delete(int masterId, string dateTicks, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var result = _svHealth.DeleteMaster(masterId, User.Identity.Name);
                if (result)
                {
                    return RedirectToAction("AuditListSearch", new { pageIndex, pageSize });
                }
                else
                {
                    throw new Exception("操作失败");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("OrderInfo", new { masterId, dateTicks, isDelete = true, pageIndex, pageSize });
            }
        }
        /// <summary>
        /// OP  审核订单
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isLookInfo">是否是查看订单信息</param>
        /// <returns></returns>
        public ActionResult AuditOrder(int masterId, string dateTicks, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var model = _svHealth.GetHealthAuditOrder(masterId, dateTicks);
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
                if (master.DateTicks != model.DateTicks) RedirectToAction("AuditListSearch", new { model.PageIndex, model.PageSize });
                var result = Request.Form["result"] == "1";
                master.Status = result ? 11 : 14;
                master.BaokuConfirmDate = DateTime.Now;
                master.BaokuConfirmer = User.Identity.Name;
                _svHealth.UpdateMaster(master);
            }
            return RedirectToAction("AuditListSearch", new { model.PageIndex, model.PageSize });
        }

        //财务确认收款
        public ActionResult FnComfirm(int masterId, string dateTicks, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var model = new VFNConfirm() { MasterId = masterId, DateTicks = dateTicks, PageIndex = pageIndex, PageSize = pageSize };
                model.FinanceAmount = _svHealth.GetConfirmPayment(masterId, dateTicks).Amount;
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
                if (model.FinancePayDate > DateTime.Now) return RedirectToAction("AuditListSearch", new { model.PageIndex, model.PageSize });
                var master = _svHealth.GetHealthMaster(model.MasterId, dateTicks: model.DateTicks);

                master.FinanceAmount = model.FinanceAmount;
                master.FinanceBankSerialNumber = model.FinanceBankSerialNumber;
                master.FinanceMemo = model.FinanceMemo;
                master.FinancePayDate = model.FinancePayDate;
                master.FinanceConfirmDate = DateTime.Now;
                master.FinanceConfirmer = User.Identity.Name;
                master.Status = 17;
                master.ServicePeriod = model.FinancePayDate.AddDays(180);
                //master.BaokuOrderCode = "HLTH" + string.Format("{0:0000000000}", master.Id);
                _svHealth.UpdateMaster(master);

                var mailContent = $"体检订单：{ master.BaokuOrderCode}已确认付款";
                var mailTo = _genericAttributeService.GetByGroup("HealthFinanceMailTo").Select(c => c.Value);
                MailService.SendMailAsync(new MailQueue()
                {
                    MQTYPE = "HealthOrder",
                    MQSUBJECT = "体检订单确认付款通知",
                    MQMAILCONTENT = "",
                    MQMAILFRM = "service@inscoo.com",
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
        [AllowAnonymous]
        public ActionResult HealthPersons(int masterId, long ticks, int pageIndex = 1, int pageSize = 15)
        {
            if (masterId > 0)
            {
                var model = _svHealth.GetHealthOrderDetails(pageIndex, pageSize, masterId, ticks);
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
        public ActionResult UploadEmp(int id)
        {

            var order = _svHealth.GetHealthMaster(id);
            ViewBag.id = id;
            ViewBag.ticks = order.DateTicks;
            return View();
        }
        /// <summary>
        /// 上传名单
        /// </summary>
        /// <param name="empinfo"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadEmp(HttpPostedFileBase empinfo, int id, string ticks)
        {
            try
            {
                var mes = _svHealth.UploadEmpExcel(empinfo, id, User.Identity.Name);
                if (!string.IsNullOrEmpty(mes)) TempData["error"] = mes;
            }
            catch (WarningException e)
            {
                TempData["error"] = e.Message;

            }
            catch (Exception e)
            {
                TempData["error"] = "上传失败！";
            }
            return RedirectToAction("OrderInfo", new { masterId = id, dateTicks = ticks });
        }
        /// <summary>
        /// 确认付款
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public ActionResult ConfirmPayment(string dateTicks)
        {
            if (!string.IsNullOrEmpty(dateTicks))
            {
                var item = _svHealth.GetByTicks(dateTicks, User.Identity.Name);
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.FirstOrDefault().PaymentNoticePdf))//还未产生付款通知书
                    {
                        item.FirstOrDefault().PaymentNoticePdf = _svHealth.GetPaymentNoticePdf(dateTicks);
                    }
                    var model = new VHealthConfirmPayment();
                    model.Amount = item.Sum(i => i.SellPrice);
                    model.PaymentNoticePdf = item.FirstOrDefault().PaymentNoticePdf;
                    model.Ticks = item.FirstOrDefault().DateTicks;
                    model.prodList = new List<VCheckProductDetail>();
                    foreach (var p in item)
                    {
                        var prod = _svHealth.GetHealthProductById(p.HealthCheckProductId, User.Identity.GetUserId());
                        prod.Count = p.Count;
                        prod.SubTotal = prod.Count * prod.PrivilegePrice;
                        model.prodList.Add(prod);
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
        public PartialViewResult AuditListData(bool isInscooOperator, bool isFinance, string listType, int pageIndex = 1, int pageSize = 15)
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
            ViewBag.author = User.Identity.Name;
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