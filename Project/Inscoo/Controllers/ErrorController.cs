using System.Web.Mvc;

namespace Inscoo.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult InternalError()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}