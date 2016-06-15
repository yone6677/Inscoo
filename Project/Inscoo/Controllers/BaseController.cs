using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
       
    }
    public class InscooAuthorize: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
        }
    }
}