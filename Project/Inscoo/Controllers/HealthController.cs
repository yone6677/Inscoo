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
using Domain.Health;

namespace Inscoo.Controllers
{
    public class HealthController : BaseController
    {
        private readonly IHealthService _svHealth;
        private readonly ICompanyService _svCompany;
        private readonly AppUserManager _svAppUserManager;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWebHelper _webHelper;

        public HealthController(IGenericAttributeService genericAttributeService, AppUserManager svAppUserManager, ICompanyService svCompany, IHealthService svHealth, IWebHelper webHelper)
        {
            _genericAttributeService = genericAttributeService;
            _svAppUserManager = svAppUserManager;
            _svHealth = svHealth;
            _svCompany = svCompany;
            _webHelper = webHelper;
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
                    if (cartList.Count > 0)
                    {
                        var healthList = _svHealth.AddHealthMaster(cartList, User.Identity.Name);
                        model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                        model.DateTicks = healthList.FirstOrDefault().DateTicks;

                        var cartCookie = Request.Cookies["InscooCart"];
                        var shoppingCart = new List<CartModel>();
                        if (cartCookie != null)
                        {
                            var decryptJson = _webHelper.DecryptCookie(cartCookie.Value);
                            shoppingCart = JsonConvert.DeserializeObject<List<CartModel>>(decryptJson);
                            var cookie = new HttpCookie("InscooCart");
                            for (var i = 0; i < cartList.Count; i++)
                            {
                                shoppingCart.Remove(shoppingCart.Where(s => s.Id == cartList[i].Id).FirstOrDefault());

                                if (shoppingCart.Any())
                                {
                                    cookie.Expires = DateTime.Now.AddDays(7);
                                }
                                else
                                {
                                    cookie.Expires = DateTime.Now.AddDays(-1);
                                }
                            }
                            var cartJson = JsonConvert.SerializeObject(shoppingCart);
                            var encryStr = _webHelper.EncryptCookie(cartJson);
                            cookie.Value = encryStr;
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);
                        }

                    }

                }
                if (!string.IsNullOrEmpty(ticks))
                {
                    var healthList = _svHealth.GetByTicks(ticks, User.Identity.Name);
                    model.CompanyNameList = _svCompany.GetCompanySelectlistByUserId(User.Identity.GetUserId());
                    model.DateTicks = ticks;
                }
                return View(model);
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
                    var item = _svHealth.GetHealthMaster(masterId);
                    _svHealth.GetPaymentNoticePdfAsync(item.DateTicks);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditOrder(string DateTicks, string result)
        {

            if (ModelState.IsValid)
            {
                var list = _svHealth.GetByTicks(DateTicks).ToList();
                var auditResult = result == "1";
                foreach (var o in list)
                {
                    var item = _svHealth.GetHealthMaster(o.Id);
                    item.Status = auditResult ? 11 : 14;
                    item.BaokuConfirmDate = DateTime.Now;
                    item.BaokuConfirmer = User.Identity.Name;
                    _svHealth.UpdateMaster(item);
                }
            }
            return RedirectToAction("Audit", new { Ticks = DateTicks });
        }

        //财务确认收款
        public ActionResult FnComfirm(string Ticks)
        {
            try
            {
                var model = new VFNConfirm() { DateTicks = Ticks };

                var list = _svHealth.GetByTicks(Ticks);
                decimal amount = 0;
                foreach (var a in list)
                {
                    amount += a.SellPrice * a.Count;
                }
                model.FinanceAmount = amount;
                return PartialView(model);

            }
            catch (Exception)
            {
                return RedirectToAction("AuditListSearch");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FnComfirm(VFNConfirm model)
        {

            if (ModelState.IsValid)
            {
                var query = _svHealth.GetByTicks(model.DateTicks);
                var list = query.ToList();
                if (list.Any())
                {
                    foreach (var a in list)
                    {
                        var item = _svHealth.GetHealthMaster(a.Id);
                        item.FinanceAmount = item.Count * item.SellPrice;
                        item.FinanceBankSerialNumber = model.FinanceBankSerialNumber;
                        item.FinanceMemo = model.FinanceMemo;
                        item.FinancePayDate = model.FinancePayDate;
                        item.FinanceConfirmDate = DateTime.Now;
                        item.FinanceConfirmer = User.Identity.Name;
                        item.Status = 17;
                        item.ServicePeriod = model.FinancePayDate.AddDays(180);
                        _svHealth.UpdateMaster(item);
                    }
                }
                var mailContent = $"体检订单：{ list.FirstOrDefault().BaokuOrderCode}已确认付款";
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
            return RedirectToAction("Audit", new { Ticks = model.DateTicks });
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
                ViewBag.id = masterId;
                ViewBag.ticks = ticks;
                return PartialView(model);
            }
            return null;
        }
        public ActionResult HealthEmpTemp(int masterId, long ticks, int pageIndex = 1, int pageSize = 15)
        {
            if (masterId > 0)
            {
                var model = _svHealth.GetHealthEmpTemp(pageIndex, pageSize, masterId, ticks);
                var command = new PageCommand()
                {
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    TotalCount = model.TotalCount,
                    TotalPages = model.TotalPages
                };
                ViewBag.pageCommand = command;
                ViewBag.id = masterId;
                ViewBag.ticks = ticks;
                return PartialView(model);
            }
            return null;
        }
        public ActionResult DeleteEmp(int id, string MasterId, string DateTicks)
        {
            _svHealth.DeleteHealthEmp(id);
            return RedirectToAction("OrderInfo", new { masterId = MasterId, dateTicks = DateTicks });
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
            return RedirectToAction("UploadEmp", new { id = id });
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
        public ActionResult Audit(string Ticks)
        {
            var list = _svHealth.GetByTicks(Ticks);
            var model = new VHealthAuditOrder();
            model.product = new List<VCheckProductDetail>();
            if (list.Any())
            {
                decimal amount = 0;
                foreach (var a in list)
                {
                    amount += a.SellPrice * a.Count;
                    var item = new VCheckProductDetail()
                    {
                        ProductName = a.HealthCheckProduct.ProductName,
                        CompanyName = a.HealthCheckProduct.CompanyName,
                        Count = a.Count,
                        PublicPrice = a.PublicPrice,
                        PrivilegePrice = a.SellPrice,
                        SubTotal = a.SellPrice * a.Count
                    };
                    model.product.Add(item);
                }
                var ex = list.FirstOrDefault();
                model.CompanyName = ex.Company.Name;
                model.Linkman = ex.Company.LinkMan;
                model.PhoneNumber = ex.Company.Phone;
                model.Address = ex.Company.Address;
                model.Amount = amount;
                model.CreateTime = ex.CreateTime;
                model.Author = ex.Author;
                model.State = ex.Status;
                model.DateTicks = Ticks;
                if (ex.FinanceConfirmDate.HasValue)
                {
                    model.ServicePeriod = ex.FinanceConfirmDate.Value.ToShortDateString() + "至" + ex.ServicePeriod.Value.ToShortDateString();
                }
                model.Expire = ex.Expire;
                model.IsInscooOperator = _svAppUserManager.GetRoles(User.Identity.GetUserId()).Contains("InscooOperator");
                model.IsFinance = _svAppUserManager.GetRoles(User.Identity.GetUserId()).Contains("InscooFinance");
                // ViewBag.role=User.Identity.get
            }
            return View(model);
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
            ViewBag.author = User.Identity.Name;
            return PartialView(list);
        }
        public ActionResult HealthMgr()
        {
            return View();
        }
        /// <summary>
        /// 健康管理列表
        /// </summary>
        public PartialViewResult List(DateTime? beginDate = null, DateTime? endDate = null, string productName = null, string orderNumber = null, int pageIndex = 1, int pageSize = 15)
        {
            var list = _svHealth.GetHealthList(17, beginDate, endDate, productName, orderNumber, pageIndex, pageSize);
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
        public ActionResult PutEmp(int id)
        {
            var master = _svHealth.GetHealthMaster(id);
            _svHealth.PutEmp(id);
            return RedirectToAction("OrderInfo", new { masterId = master.Id, dateTicks = master.DateTicks });
        }
    }
}