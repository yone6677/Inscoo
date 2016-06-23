using Domain.Orders;
using Innscoo.Infrastructure;
using Models.Order;
using Models.Products;
using OfficeOpenXml;
using Services;
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
        private readonly IFileService _fileService;
        private readonly IOrderEmpService _orderEmpService;
        public OrderController(IMixProductService mixProductService, IAppUserService appUserService, IGenericAttributeService genericAttributeService,
            IProductService productService, IOrderService orderService, IOrderItemService orderItemService, IFileService fileService, IOrderEmpService orderEmpService)
        {
            _mixProductService = mixProductService;
            _appUserService = appUserService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
            _orderItemService = orderItemService;
            _orderService = orderService;
            _fileService = fileService;
            _orderEmpService = orderEmpService;
        }
        // GET: Oder
        public ActionResult Index()
        {
            return View();
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
                var order = new Order();
                order.AgeRange = model.AgeRange;
                order.Amount = 0;
                order.AnnualExpense = model.AnnualExpense;
                order.CommissionType = null;//_appUserService.GetCurrentUser().;佣金计算方法，稍后在用户表增加
                order.FanBao = model.FanBao;
                order.Memo = model.Memo;
                order.Name = model.OrderName;
                order.Pretium = model.pretium;
                order.Rebate = _appUserService.GetCurrentUser().Rebate;//Rebate
                order.StaffRange = model.StaffRange;
                order.TiYong = model.TiYong;
                int result = _orderService.Insert(order);
                if (result > 0)//写产品
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
                                CommissionRate = null,
                                CoverageSum = item.CoverageSum,
                                InsuredWho = item.InsuredWho,
                                OId = result,
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
                    var model = new EntryInfoModel()
                    {
                        Id = id,                       
                        Linkman = order.Linkman,
                        CompanyName = order.CompanyName,
                        PhoneNumber = order.PhoneNumber,
                        Address = order.Address
                    };
                    if(order.StartDate==DateTime.MinValue)
                    {
                        model.StartDate = order.CreateTime.AddDays(4);
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
                    entity.State = 2;
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
        public ActionResult UploadFile(int id)
        {
            if (id > 0)
            {
                var order = _orderService.GetById(id);
                if (order.State > 1)
                {
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public void DownloadEmpInfo()
        {
            var url = "/Archive/Template/上传人员信息.xlsx";
            _fileService.DownloadFile(url, "人员信息.xlsx");
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
                //若有旧数据先删除
                var oldInfo = _orderEmpService.GetList(Id);
                if (oldInfo != null && oldInfo.Any())
                {
                    foreach (var s in oldInfo)
                    {
                        _orderEmpService.DeleteById(s.Id);
                    }
                }
                _fileService.SaveFile(empinfo);
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
                    item.OId = Id;
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
                    if (!_orderEmpService.Insert(item))
                    {
                        result = "上传失败";
                    }
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
    }
}