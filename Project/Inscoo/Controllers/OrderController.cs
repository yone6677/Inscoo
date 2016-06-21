using Models.Order;
using Models.Products;
using Services.Identity;
using Services.Products;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMixProductService _mixProductService;
        private readonly IAppUserService _appUserService;
        public OrderController(IMixProductService mixProductService, IAppUserService appUserService)
        {
            _mixProductService = mixProductService;
            _appUserService = appUserService;
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
                    cOrder.AnnualExpense = mixProdt.Price;
                    foreach (var s in mixProdt.ProductMixItem)
                    {
                        var pitem = new ProductModel()
                        {
                            Id = s.product.Id,
                            CoverageSum = s.CoverageSum,
                            PayoutRatio = s.PayoutRatio,
                            Price = s.OriginalPrice,
                            SafeguardName = s.SafefuardName,
                            ProdType = s.product.ProdType,
                            SafeguardCode = s.product.SafeguardCode,
                            InsuredWho = s.product.InsuredWho
                        };
                        cOrder.ProdItem.Add(pitem);
                        cOrder.UserRole = _appUserService.GetUserRoles();
                    }
                    return View(cOrder);
                }
            }
            if (!string.IsNullOrEmpty(model.productIds))//自选产品
            {
                return Content(model.productIds);
            }
            return null;
        }
        public ActionResult Buy()
        {
            var model = new ConfirmOrderModel();
            return View(model);
        }
    }
}