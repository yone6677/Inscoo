using Domain.Orders;
using Models.Order;
using Models.Products;
using Services.Common;
using Services.Identity;
using Services.Orders;
using Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public OrderController(IMixProductService mixProductService, IAppUserService appUserService, IGenericAttributeService genericAttributeService,
            IProductService productService, IOrderService orderService, IOrderItemService orderItemService)
        {
            _mixProductService = mixProductService;
            _appUserService = appUserService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
            _orderItemService = orderItemService;
            _orderService = orderService;
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
                    var cOrder = new ConfirmOrderModel();
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
                var cOrder = new ConfirmOrderModel();
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
                order.Rebate = model.Rebate;
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
                }
            }
            return null;
        }
    }
}