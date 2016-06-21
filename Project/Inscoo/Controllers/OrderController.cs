using Models.Order;
using Models.Products;
using Services.Common;
using Services.Identity;
using Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMixProductService _mixProductService;
        private readonly IAppUserService _appUserService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;
        public OrderController(IMixProductService mixProductService, IAppUserService appUserService, IGenericAttributeService genericAttributeService, IProductService productService)
        {
            _mixProductService = mixProductService;
            _appUserService = appUserService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
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
                    cOrder.OrderName = mixProdt.Name;
                    cOrder.StaffRange = mixProdt.StaffRange;
                    cOrder.AgeRange = mixProdt.AgeRange;
                    cOrder.AnnualExpense = mixProdt.Price.ToString();
                    cOrder.UserRole = _appUserService.GetUserRoles();
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

                    }
                    return View(cOrder);
                }
            }
            //自选产品
            if (!string.IsNullOrEmpty(model.productIds) && !string.IsNullOrEmpty(model.companyName) && !string.IsNullOrEmpty(model.StaffsNum) && !string.IsNullOrEmpty(model.Avarage))
            {
                var cOrder = new ConfirmOrderModel();
                cOrder.UserRole = _appUserService.GetUserRoles();
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
                List<string> li = new List<string>();
                if (model.productIds.Contains(","))
                {
                    string[] tempStr = model.productIds.Split(',');
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        li.Add(tempStr[i].Trim());
                    }
                }
                else
                {
                    li.Add(model.productIds);
                }
                foreach (var s in li)
                {
                    var pitem = _productService.GetById(int.Parse(s));
                    if (pitem != null)
                    {
                        var item = new ProductModel()
                        {
                            Id = pitem.Id,
                            CoverageSum = pitem.CoverageSum,
                            PayoutRatio = pitem.PayoutRatio,
                            SafeguardName = pitem.SafeguardName,
                            ProdType = pitem.ProdType,
                            SafeguardCode = pitem.SafeguardCode,
                            InsuredWho = pitem.InsuredWho,
                        };
                        string price = "";
                        int stafnum = int.Parse(model.StaffsNum);
                        int avage = int.Parse(model.Avarage);
                        if (stafnum > 0 && avage > 0)
                        {
                            switch (stafnum)
                            {
                                case 1:
                                    price = pitem.HeadCount3;
                                    break;
                                case 2:
                                    price = pitem.HeadCount5;
                                    break;
                                case 3:
                                    price = pitem.HeadCount11;
                                    break;
                                case 4:
                                    price = pitem.HeadCount31;
                                    break;
                                case 5:
                                    price = pitem.HeadCount51;
                                    break;
                                case 6:
                                    price = pitem.HeadCount100;
                                    break;
                            }
                            item.OriginalPrice = price;//原价
                            if (price.Trim() != "-")
                            {
                                double pr = double.Parse(price);
                                switch (avage)
                                {
                                    case 2:
                                        price = (pr * 1.1).ToString();
                                        break;
                                    case 3:
                                        price = (pr * 1.2).ToString();
                                        break;
                                    case 4:
                                        price = (pr * 1.3).ToString();
                                        break;
                                }
                                item.Price = price;//实际售价
                            }
                        }
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
    }
}