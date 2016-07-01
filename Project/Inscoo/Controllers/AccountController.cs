using Models;
using Services;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        public AccountController(IAppUserService appUserService, IAppRoleService appRoleService)
        {
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }
        // GET: Account
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _appUserService.Find(model.UserName, model.Password);
                if (user != null)
                {
                    _appUserService.SignIn(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }
        public ActionResult SignOut()
        {
            _appUserService.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}