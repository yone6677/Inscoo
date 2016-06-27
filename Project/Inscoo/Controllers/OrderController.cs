using Domain.Orders;
using Innscoo.Infrastructure;
using Models.Infrastructure;
using Models.Order;
using Models.Products;
using OfficeOpenXml;
using Services;
using Services.Archives;
using Services.Common;
using Services.Identity;
using Services.Orders;
using Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IMixProductService _mixProductService;
        private readonly IAppUserService _appUserService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IArchiveService _archiveService;
        private readonly IOrderEmpService _orderEmpService;
        private readonly IOrderBatchService _orderBatchService;
        private readonly IResourceService _resourceService;
        private readonly IAppRoleService _appRoleService;
        public OrderController(IMixProductService mixProductService, IAppUserService appUserService, IGenericAttributeService genericAttributeService, IProductService productService,
            IOrderService orderService, IOrderItemService orderItemService, IArchiveService archiveService, IOrderEmpService orderEmpService, IOrderBatchService orderBatchService,
            IResourceService resourceService, IAppRoleService appRoleService)
        {
            _mixProductService = mixProductService;
            _appUserService = appUserService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
            _orderItemService = orderItemService;
            _orderService = orderService;
            _archiveService = archiveService;
            _orderEmpService = orderEmpService;
            _orderBatchService = orderBatchService;
            _resourceService = resourceService;
            _appRoleService = appRoleService;
        }
        // GET: Oder
        public ActionResult Index()
        {
            var select = _genericAttributeService.GetSelectList("orderState", true);
            ViewBag.orderState = select;
            return View();
        }

        public ActionResult List()
        {
            var model = _orderService.GetListOfPager(1, 15);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(int PageIndex = 1, int PageSize = 15, string name = null, int state = 0, string companyName = null, DateTime? beginDate = null, DateTime? endDate = null)
        {
            var model = _orderService.GetListOfPager(PageIndex, PageSize, name, state, companyName, beginDate, endDate);
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

        [HttpPost]
        public ActionResult Buy(CustomizeBuyModel model)
        {
            if (model.CustomizeProductId > 0)//推荐产品
            {
                var mixProdt = _mixProductService.GetById(model.CustomizeProductId);
                if (mixProdt != null)
                {
                    var cuser = _appUserService.GetCurrentUser();
                    var cOrder = new ConfirmOrderModel();
                    cOrder.HasFanBao = cuser.FanBao;
                    cOrder.HasTiYong = cuser.TiYong;
                    cOrder.UserRole = _appUserService.GetUserRoles();
                    if (cOrder.UserRole.Count > 0)//获取用户返点
                    {
                        foreach (var u in cOrder.UserRole)
                        {
                            if (u.RoleName == "Admin" || u.RoleName == "PartnerChannel")
                            {
                                var user = _appUserService.GetCurrentUser();
                                if (user != null)
                                {
                                    cOrder.UserRebate = user.Rebate;
                                }
                            }
                        }
                    }
                    cOrder.OrderName = mixProdt.Name;
                    cOrder.StaffRange = mixProdt.StaffRange;
                    cOrder.AgeRange = mixProdt.AgeRange;
                    cOrder.AnnualExpense = mixProdt.Price;
                    int i = 0;
                    foreach (var s in mixProdt.ProductMixItem)
                    {
                        var pitem = new ProductModel()
                        {
                            Id = s.product.Id,
                            CoverageSum = s.CoverageSum,
                            PayoutRatio = s.PayoutRatio,
                            Price = s.OriginalPrice.ToString(),
                            SafeguardName = s.SafefuardName,
                            ProdType = s.product.ProdType,
                            SafeguardCode = s.product.SafeguardCode,
                            InsuredWho = s.product.InsuredWho
                        };
                        cOrder.ProdItem.Add(pitem);
                        if (i == 0)
                        {
                            cOrder.ids += s.Id;
                        }
                        else
                        {
                            cOrder.ids += "," + s.Id;
                        }
                        i++;
                    }
                    return View(cOrder);
                }
            }
            //自选产品
            if (!string.IsNullOrEmpty(model.productIds) && !string.IsNullOrEmpty(model.companyName) && !string.IsNullOrEmpty(model.StaffsNum) && !string.IsNullOrEmpty(model.Avarage))
            {
                var cuser = _appUserService.GetCurrentUser();
                var cOrder = new ConfirmOrderModel();
                cOrder.HasFanBao = cuser.FanBao;
                cOrder.HasTiYong = cuser.TiYong;
                cOrder.UserRole = _appUserService.GetUserRoles();
                if (cOrder.UserRole.Count > 0)//获取用户返点
                {
                    foreach (var u in cOrder.UserRole)
                    {
                        if (u.RoleName == "Admin" || u.RoleName == "PartnerChannel")
                        {
                            var user = _appUserService.GetCurrentUser();
                            if (user != null)
                            {
                                cOrder.UserRebate = user.Rebate;
                            }
                        }
                    }
                }

                cOrder.AnnualExpense = decimal.Parse(model.Price);
                var staffsNumber = _genericAttributeService.GetList("StaffRange").Where(g => g.Value == model.StaffsNum);
                if (staffsNumber.Count() > 0)
                {
                    cOrder.StaffRange = staffsNumber.FirstOrDefault().Key;
                }
                var AgeRange = _genericAttributeService.GetList("AgeRange").Where(g => g.Value == model.Avarage);
                if (staffsNumber.Count() > 0)
                {
                    cOrder.AgeRange = AgeRange.FirstOrDefault().Key;
                }
                cOrder.ids = model.productIds;
                List<int> li = new List<int>();
                if (model.productIds.Contains(","))
                {
                    string[] tempStr = model.productIds.Split(',');
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        li.Add(int.Parse(tempStr[i].Trim()));
                    }
                }
                else
                {
                    li.Add(int.Parse(model.productIds));
                }
                foreach (var s in li)
                {
                    var item = _productService.GetProductPrice(s, null, int.Parse(model.StaffsNum), int.Parse(model.Avarage));
                    if (item != null)
                    {
                        cOrder.ProdItem.Add(item);
                    }
                }
                return View(cOrder);
            }
            throw new Exception("选择方案失败");
        }
        public ActionResult Buy()
        {
            var model = new ConfirmOrderModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ConfirmOrderModel model)
        {
            if (ModelState.IsValid)
            {
                List<int> li = new List<int>();
                if (model.ids.Contains(","))
                {
                    string[] tempStr = model.ids.Split(',');
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        li.Add(int.Parse(tempStr[i].Trim()));
                    }
                }
                else
                {
                    li.Add(int.Parse(model.ids));
                }
                var order = new Order();
                order.AgeRange = model.AgeRange;
                order.AnnualExpense = model.AnnualExpense;
                order.CommissionType = null;//_appUserService.GetCurrentUser().;佣金计算方法，稍后在用户表增加
                order.FanBao = model.FanBao;
                order.Memo = model.Memo;
                order.Name = model.OrderName;
                order.Pretium = model.pretium;
                order.Rebate = _appUserService.GetCurrentUser().Rebate;//Rebate
                order.StaffRange = model.StaffRange;
                order.TiYong = model.TiYong;
                order.Insurer = _productService.GetById(int.Parse(li[0].ToString())).InsuredCom;//保险公司名称
                int result = _orderService.Insert(order);
                if (result > 0)//写产品&批次
                {
                    var ob = new OrderBatch()
                    {
                        order_Id = result,
                        BState = 0,
                        PolicyHolder = _appUserService.GetCurrentUser().Id,
                        PolicyHolderDate = DateTime.Now
                    };
                    _orderBatchService.Insert(ob);
                    int staffnum = 0;
                    int avarag = 0;
                    var staff = _genericAttributeService.GetByKey(model.StaffRange, "StaffRange");
                    var avar = _genericAttributeService.GetByKey(model.AgeRange, "AgeRange");
                    if (staff != null)
                    {
                        staffnum = int.Parse(staff.Value);
                    }
                    if (avar != null)
                    {
                        avarag = int.Parse(avar.Value);
                    }
                    foreach (var s in li)
                    {
                        var item = _productService.GetProductPrice(s, null, staffnum, avarag);
                        if (item != null)
                        {
                            var orderItem = new OrderItem()
                            {
                                CommissionRate = _productService.GetById(item.Id).CommissionRate,//佣金比率
                                CoverageSum = item.CoverageSum,
                                InsuredWho = item.InsuredWho,
                                order_Id = result,
                                OriginalPrice = decimal.Parse(item.OriginalPrice),
                                PayoutRatio = item.PayoutRatio,
                                Price = decimal.Parse(item.Price),
                                ProdType = item.ProdType,
                                SafeguardCode = item.SafeguardCode,
                                SafeguardName = item.SafeguardName
                            };
                            _orderItemService.Insert(orderItem);
                        }
                    }
                    return RedirectToAction("EntryInfo", new { id = result });//redirect entry infomation
                }
            }
            return null;
        }

        public ActionResult EntryInfo(int id)
        {
            if (id > 0)
            {
                var order = _orderService.GetById(id);
                if (order.State > 0)
                {
                    var empCount = _orderEmpService.GetListByOid(id);
                    var model = new EntryInfoModel()
                    {
                        Id = id,
                        Linkman = order.Linkman,
                        CompanyName = order.CompanyName,
                        PhoneNumber = order.PhoneNumber,
                        Address = order.Address,
                        IsUploadInfo = empCount.Count,//已上传人员数
                        EmpInfoFileUrl = _resourceService.GetEmployeeInfoTemp()
                    };
                    if (order.StartDate == DateTime.MinValue)
                    {
                        model.StartDate = order.CreateTime.AddDays(3);
                    }
                    return View(model);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntryInfo(EntryInfoModel model)
        {
            if (ModelState.IsValid)
            {

                var entity = _orderService.GetById(model.Id);
                if (entity != null)
                {
                    entity.CompanyName = model.CompanyName;
                    entity.Linkman = model.Linkman;
                    entity.PhoneNumber = model.PhoneNumber;
                    entity.Address = model.Address;
                    entity.StartDate = model.StartDate;
                    entity.EndDate = model.StartDate.AddYears(1).AddDays(-1);
                    entity.State = 2;//已完成人员上传
                    if (_orderService.Update(entity))
                    {
                        return RedirectToAction("UploadFile", new { id = model.Id });
                    }
                    else
                    {
                        ViewBag.Error = "遇到错误，请检查输入";
                    }
                }
            }
            return View(model);
        }
        /// <summary>
        /// 完成第三步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(UploadInfoModel model)
        {
            if (model.Id > 0)
            {
                var order = _orderService.GetById(model.Id);
                if (order != null)
                {
                    order.State = 3;
                    if (_orderService.Update(order))
                    {
                        return RedirectToAction("ConfirmPayment", new { id = model.Id });
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult UploadFile(int id)
        {
            if (id > 0)
            {
                var model = new UploadInfoModel();
                model.Id = id;
                model.InsurancePolicyTemp = _resourceService.GetInsurancePolicyTemp();
                var order = _orderService.GetById(id);
                if (order.State > 1)
                {
                    var orderBatch = _orderBatchService.GetByOrderId(id);
                    if (orderBatch != null)
                    {
                        if (orderBatch.EmpInfoFilePDF == 0)//还未生成PDF
                        {
                            var virpath = _orderEmpService.GetPdf(id);//产生PDF文件
                            if (virpath.Count > 0)
                            {
                                var fid = _archiveService.InsertByUrl(virpath, FileType.EmployeeInfoSeal.ToString(), id, "人员信息PDF");
                                orderBatch.EmpInfoFilePDF = fid;
                                if (_orderBatchService.Update(orderBatch))
                                {
                                    model.EmpInfoFilePDFUrl = virpath[1];
                                }
                            }
                        }
                        else//已生成PDF
                        {
                            var archive = _archiveService.GetById(orderBatch.EmpInfoFilePDF);
                            model.EmpInfoFilePDFUrl = archive.Url;
                            if (orderBatch.EmpInfoFileSeal > 0)//已上传人员信息PDF加盖公章
                            {
                                model.HasEmpInfoFilePDFSeal = true;
                                model.EmpInfoFilePDFSealUrl = _archiveService.GetById(orderBatch.EmpInfoFileSeal).Url;
                            }
                            if (order.BusinessLicense > 0)//已上传营业执照
                            {
                                model.HasBusinessLicense = true;
                                model.BusinessLicenseSealUrl = _archiveService.GetById(order.BusinessLicense).Url;
                            }
                            if (orderBatch.PolicySeal > 0)//已上传投保单
                            {
                                model.HasInsurancePolicy = true;
                                model.InsurancePolicySealUrl = _archiveService.GetById(orderBatch.PolicySeal).Url;
                            }
                        }
                        return View(model);
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult UploadEmp(int Id, int PageIndex = 1, int PageSize = 15)
        {
            if (Id > 0)
            {
                var model = _orderEmpService.GetListOfPager(PageIndex, PageSize, Id);
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

        [HttpPost]
        public ActionResult UploadEmp(HttpPostedFileBase empinfo, int Id)
        {
            var result = "";
            if (empinfo != null && Id > 0)
            {
                var order = _orderService.GetById(Id);
                var batch = _orderBatchService.GetByOrderId(Id);
                //若有旧数据先删除
                var oldInfo = _orderEmpService.GetListByOid(Id);
                if (oldInfo != null && oldInfo.Any())
                {
                    foreach (var s in oldInfo)
                    {
                        _orderEmpService.DeleteById(s.Id);
                    }
                }
                var fileModel = _archiveService.Insert(empinfo, FileType.EmployeeInfo.ToString(), Id);
                /***暂时写这里*/
                var ep = new ExcelPackage(empinfo.InputStream);
                var worksheet = ep.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    result = "上传的文件内容不能为空";
                var rowNumber = worksheet.Dimension.Rows;
                var Cells = worksheet.Cells;
                if (Cells["A1"].Value.ToString() != "被保险人姓名" || Cells["B1"].Value.ToString() != "证件类型" || Cells["C1"].Value.ToString() != "证件号码" || Cells["D1"].Value.ToString() != "生日" || Cells["E1"].Value.ToString() != "性别(男/女)" || Cells["F1"].Value.ToString() != "银行账号" || Cells["G1"].Value.ToString() != "开户行" || Cells["H1"].Value.ToString() != "联系电话" || Cells["I1"].Value.ToString() != "邮箱" || Cells["J1"].Value.ToString() != "社保（有/无）")
                    result = "上传的文件不正确";
                var eList = new List<OrderEmployeeModel>();
                for (var i = 2; i <= rowNumber; i++)
                {
                    if (Cells["A" + i].Value == null)
                        break;
                    var item = new OrderEmployee();
                    item.batch_Id = batch.Id;//批次号
                    item.Premium = order.AnnualExpense;
                    item.PMCode = PMType.PM00.ToString();
                    item.Name = Cells["A" + i].Value.ToString().Trim();
                    item.IDType = Cells["B" + i].Value.ToString().Trim();
                    item.IDNumber = Cells["C" + i].Value.ToString().Trim();
                    item.BirBirthday = DateTime.Parse(Cells["D" + i].Value.ToString().Trim());
                    item.Sex = Cells["E" + i].Value.ToString().Trim();
                    item.BankCard = Cells["F" + i].Value.ToString().Trim();
                    item.BankName = Cells["G" + i].Value.ToString().Trim();
                    item.PhoneNumber = Cells["H" + i].Value.ToString().Trim();
                    item.Email = Cells["I" + i].Value.ToString().Trim();
                    item.HasSocialSecurity = Cells["J" + i].Value.ToString().Trim();
                    item.StartDate = order.StartDate;
                    item.EndDate = order.EndDate;
                    if (!_orderEmpService.Insert(item))
                    {
                        result = "上传失败";
                    }
                }
                order.InsuranceNumber = rowNumber - 1; //更新订单主表投保人数
                _orderService.Update(order);
                //定单批次
                var orderBatch = _orderBatchService.GetByOrderId(Id);
                if (orderBatch != null)
                {
                    orderBatch.EmpInfoFile = fileModel;
                    _orderBatchService.Update(orderBatch);
                }
                return RedirectToAction("EntryInfo", new { id = Id });
                /****/
            }
            else
            {
                result = "上传失败";
            }
            return Content(result);
        }
        [HttpPost]
        public ActionResult UploadEmpInfoPdf(HttpPostedFileBase EmpInfoPdfSeal, int Id)
        {
            if (EmpInfoPdfSeal != null && Id > 0)
            {
                var orderBatch = _orderBatchService.GetByOrderId(Id);
                if (orderBatch != null)
                {
                    var fileId = _archiveService.Insert(EmpInfoPdfSeal, FileType.EmployeeInfoSeal.ToString(), Id);
                    if (fileId > 0)
                    {
                        orderBatch.EmpInfoFileSeal = fileId;
                        if (_orderBatchService.Update(orderBatch))
                        {
                            return RedirectToAction("UploadFile", new { id = Id });
                        }
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult UploadPolicyPdfSeal(HttpPostedFileBase PolicyPdfSeal, int Id)
        {
            if (PolicyPdfSeal != null && Id > 0)
            {
                var orderBatch = _orderBatchService.GetByOrderId(Id);
                if (orderBatch != null)
                {
                    var fileId = _archiveService.Insert(PolicyPdfSeal, FileType.PolicySeal.ToString(), Id);
                    if (fileId > 0)
                    {
                        orderBatch.PolicySeal = fileId;
                        if (_orderBatchService.Update(orderBatch))
                        {
                            return RedirectToAction("UploadFile", new { id = Id });
                        }
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult UploadBusinessLicensePdf(HttpPostedFileBase BusinessLicensePdfSeal, int Id)
        {
            if (BusinessLicensePdfSeal != null && Id > 0)
            {
                var order = _orderService.GetById(Id);
                if (order != null)
                {
                    var fileId = _archiveService.Insert(BusinessLicensePdfSeal, FileType.BusinessLicenseSeal.ToString(), Id);
                    if (fileId > 0)
                    {
                        order.BusinessLicense = fileId;
                        if (_orderService.Update(order))
                        {
                            return RedirectToAction("UploadFile", new { id = Id });
                        }
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        /// <summary>
        /// 第四步,确认付款页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmPayment(int id)
        {
            if (id > 0)
            {
                var model = new ConfirmPaymentModel();
                var order = _orderService.GetById(id);
                var orderBatch = _orderBatchService.GetByOrderId(id);
                if (order != null && orderBatch != null)
                {
                    if (orderBatch.PaymentNoticePDF == 0)//还未产生付款通知书
                    {
                        var ls = _orderEmpService.GetPaymentNoticePdf(id);
                        var fid = _archiveService.InsertByUrl(ls, FileType.PaymentNotice.ToString(), id, "付款通知书");
                        orderBatch.PaymentNoticePDF = fid;
                        if (_orderBatchService.Update(orderBatch))
                        {
                            model.PaymentNoticeUrl = ls[1];
                            order.State = 4;//付款通知书已下载
                            _orderService.Update(order);
                        }
                    }
                    else
                    {
                        model.PaymentNoticeUrl = _archiveService.GetById(orderBatch.PaymentNoticePDF).Url;
                    }
                    model.YearPrice = order.AnnualExpense;
                    model.MonthPrice = double.Parse((order.AnnualExpense / 12).ToString());
                    model.Quantity = order.InsuranceNumber;
                    model.Amount = order.Pretium * order.InsuranceNumber;
                    return View(model);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult Details(int id)
        {
            try
            {
                var model = new OrderDatailsModel();
                var user = _appUserService.GetCurrentUser();
                var roles = _appRoleService.FindByIdAsync(user.Roles.FirstOrDefault().RoleId).Name;
                var order = _orderService.GetById(id);
                if (roles == "PartnerChannel" || roles == "CompanyHR")
                {
                    if (order.Author != user.UserName)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//安全起见，role为hr或者channel只能看自己的
                    }
                }
                model.Address = order.Address;
                model.AgeRange = order.AgeRange;
                model.AnnualExpense = order.AnnualExpense;
                model.BusinessLicense = _archiveService.GetById(order.BusinessLicense).Url;
                model.CompanyName = order.CompanyName;
                model.EndDate = order.EndDate;
                model.Id = order.Id;
                model.InsuranceNumber = order.InsuranceNumber;
                model.Insurer = order.Insurer;
                model.Linkman = order.Linkman;
                model.Memo = order.Memo;
                model.Name = order.Name;
                model.PhoneNumber = order.PhoneNumber;
                model.PolicyNumber = order.PolicyNumber;
                model.StaffRange = order.StaffRange;
                model.StartDate = order.StartDate;
                model.orderItem = _orderItemService.GetList(id).Select(p => new ProductModel
                {
                    CoverageSum = p.CoverageSum,
                    Id = p.Id,
                    InsuredWho = p.InsuredWho,
                    OriginalPrice = p.OriginalPrice.ToString(),
                    PayoutRatio = p.PayoutRatio,
                    Price = p.Price.ToString(),
                    ProdType = p.ProdType,
                    SafeguardCode = p.SafeguardCode,
                    SafeguardName = p.SafeguardName
                }).ToList();
                var orderBatch = _orderBatchService.GetList(id);
                if (orderBatch.Count > 0)
                {
                    foreach(var b in orderBatch)
                    {
                        var batchItem = new OrderBatchModel();
                        batchItem.AmountCollected = b.AmountCollected;
                        batchItem.BId = b.Id;
                        if (b.InscooConfirmDate != DateTime.MinValue)
                        {
                            batchItem.InscooConfirmDate = b.InscooConfirmDate.ToShortDateString();
                        }
                        if (b.InsurerConfirmDate != DateTime.MinValue)
                        {
                            batchItem.InsurerConfirmDate = b.InsurerConfirmDate.ToShortDateString();
                        }
                        var PaymentNoticePDF = _archiveService.GetById(b.PaymentNoticePDF);
                        if (PaymentNoticePDF != null)
                        {
                            batchItem.PaymentNoticePDF = PaymentNoticePDF.Url;
                        }
                       
                        if (b.PolicyHolderDate != DateTime.MinValue)
                        {
                            batchItem.PolicyHolderDate = b.PolicyHolderDate.ToShortDateString();
                        }
                        var PolicySeal = _archiveService.GetById(b.PolicySeal);
                        if (PolicySeal != null)
                        {
                            batchItem.PolicySeal = PolicySeal.Url;
                        }
                        model.orderBatch.Add(batchItem);
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }
            return View();
        }
    }
}