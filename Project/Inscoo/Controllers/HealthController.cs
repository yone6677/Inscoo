using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Services;

namespace Inscoo.Controllers
{
    public class HealthController : Controller
    {
        private readonly IHealthService _svHealth;

        public HealthController(IHealthService svHealth)
        {
            _svHealth = svHealth;
        }
        // GET: Health
        public ActionResult Index()
        {
            var model = _svHealth.GetHealthProducts(User.Identity.GetUserId());
            ViewBag.ProductType = model.Select(m => m.ProductType).Distinct().ToList();
            return View(model);
        }
        /// <summary>
        /// 产品信息页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BuyInfo(int id)
        {
            var model = _svHealth.GetHealthProductById(id, User.Identity.GetUserId());
            return View(model);
        }
        /// <summary>
        /// 购买流程-方案确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MakeSure(int id)
        {
            var model = _svHealth.GetHealthProductById(id, User.Identity.GetUserId());
            return View(model);
        }
        /// <summary>
        /// 购买流程-信息填写
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EntryInfo(int id)
        {
            var model = _svHealth.GetHealthProductById(id, User.Identity.GetUserId());
            return View(model);
        }
    }
}