using Models.Insurance;
using Models.Products;
using Services.Products;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class InsuranceController : Controller
    {
        private readonly IMixProductService _mixProductService;

        public InsuranceController(IMixProductService mixProductService)
        {
            _mixProductService = mixProductService;
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
            return PartialView();
        }
    }
}