﻿using System;
using System.Web;
using Models;
using Services;
using System.Web.Mvc;
using Core;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            //var appUser = _appUserService.GetAppUserManagerCore(HttpContext);
            var appUser = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var user = appUser.FindByEmail(email);
            //if (user == null) return false;
            var token = appUser.GeneratePasswordResetToken(user.Id);
            var route = new { code = token };
            var callbackUrl = Url.Action("ResetPassword", "Account", route, Request.Url.Scheme);

            MailService.SendMail(new MailQueue()
            {
                MQTYPE = "ResetPassword",
                MQSUBJECT = "重设密码",
                MQMAILCONTENT = "点击<a href=\"" + callbackUrl + "\">链接</a>重设密码",
                MQMAILFRM = "redy.yone@inscoo.com",
                MQMAILTO = user.Email,
            });
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var appUser = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var user = appUser.FindByEmail(model.Email);
                if (user == null)
                {
                    // 请不要显示该用户不存在
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                var result = appUser.ResetPassword(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}