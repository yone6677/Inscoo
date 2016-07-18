using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult AboutBaoku()
        {
            return View();
        }
        public ActionResult Question()
        {
            return View();
        }
        public ActionResult cooperation()
        {
            return View();
        }
        public ActionResult serveOrder()
        {
            return View();
        }
    }
}