using Core;
using Domain;
using Inscoo.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Services.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        public AccountController(IAppUserService appUserService, IAppRoleService appRoleService)
        {
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
        public ActionResult Register()
        {
            //var roles = _appRoleService.Roles();
            //var select = new List<SelectListItem>();
            //foreach (var s in roles)
            //{
            //    var item = new SelectListItem();
            //    item.Text = s.Description;
            //    item.Value = s.Name;
            //}
            //ViewBag.roles = select;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, LinkMan = model.Linkman, CompanyName = model.CompanyName };
                var result = _appUserService.CreateAsync(user, model.UserName, model.Password);
                if (result.Result.Succeeded)
                {
                    _appUserService.SignIn(AuthenticationManager, user,false);
                    return RedirectToLocal("/");
                }
            }
            return View();
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _appUserService.Find(model.UserName, model.Password);
                if (user != null)
                {
                     _appUserService.SignIn(AuthenticationManager, user, model.RememberMe);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 头像/NAME
        /// </summary>
        /// <returns></returns>
        public ActionResult Portrait()
        {
            ViewBag.UserName = User.Identity.Name;
            return PartialView();
        }
    }
}