using Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class MixProductController : BaseController
    {
        // GET: MixProduct
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            var model = new MixProductModel();
            return View(model);
        }
    }
}