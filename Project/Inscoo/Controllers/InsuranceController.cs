using Domain.Products;
using Models.Insurance;
using Models.Order;
using Models.Products;
using Services.Common;
using Services.Identity;
using Services.Products;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class InsuranceController : BaseController
    {
        private readonly IMixProductService _mixProductService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;       

        public InsuranceController(IMixProductService mixProductService, IGenericAttributeService genericAttributeService, IProductService productService)
        {
            _mixProductService = mixProductService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
        }
        // GET: Insurance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MixProduct()
        {
            var product = _mixProductService.GetAll();
            var model = new List<RecommendationModel>();
            foreach (var p in product)
            {
                var item = new RecommendationModel()
                {
                    Address = p.Address,
                    AgeRange = p.AgeRange,
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StaffRange = p.StaffRange
                };
                var itemModelList = new List<MixProductItemModel>();
                foreach (var s in p.ProductMixItem)
                {
                    var itemModel = new MixProductItemModel()
                    {
                        CoverageSum = s.CoverageSum,
                        Id = s.Id,
                        mid = s.mid,
                        OriginalPrice = s.OriginalPrice,
                        SafefuardName = s.SafefuardName
                    };
                    itemModelList.Add(itemModel);
                }
                item.item = itemModelList;
                model.Add(item);
            }
            return PartialView(model);
        }
        public ActionResult CustomizeProduct()
        {
            var model = new CustomProductModel();
            model.Avarage = _genericAttributeService.GetSelectList("AgeRange");
            model.StaffsNumber = _genericAttributeService.GetSelectList("StaffRange");
            model.CompanyList = _genericAttributeService.GetList("InsuranceCompany");
            return View(model);
        }
        public ActionResult ProductList(string company = null, string productType = "员工福利保险", int stuffsNum = 1)
        {
            var model = new List<ProductListModel>();
            model = _productService.GetProductListForInscoo(company, productType, stuffsNum);
            return PartialView(model);
        }
        public ActionResult Cart(string company = null)
        {
            var model = new CustomizeBuyModel()
            {
                companyName = company,
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GetProductPrice(int cid = 0, string payrat = null, int staffsNumber = 0, int avarage = 0)
        {

            var price = "0.00";
            if (cid == 0)
            {
                return null;
            }
            var model = new Product();
            var item = _productService.GetById(cid);
            if (item != null)
            {
                if (string.IsNullOrEmpty(payrat))
                {
                    model = item;
                }
                else
                {
                    var list = _productService.GetList(item.InsuredCom, item.SafeguardCode, item.CoverageSum, payrat);
                    if (list.Count > 0)
                    {
                        model = list.FirstOrDefault();
                    }
                }
            }
            if (model.Id > 0)
            {
                switch (staffsNumber)
                {
                    case 1:
                        price = model.HeadCount3;
                        break;
                    case 2:
                        price = model.HeadCount5;
                        break;
                    case 3:
                        price = model.HeadCount11;
                        break;
                    case 4:
                        price = model.HeadCount31;
                        break;
                    case 5:
                        price = model.HeadCount51;
                        break;
                    case 6:
                        price = model.HeadCount100;
                        break;
                }
            }
            if (price.Trim() != "-")
            {
                double pr = double.Parse(price);
                if (avarage > 1)
                {
                    switch (avarage)
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
                }
            }
            return new JsonResult { Data = new { id = model.Id, price = price } };
        }
    }
}