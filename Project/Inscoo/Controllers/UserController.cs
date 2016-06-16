using Core;
using Inscoo.Models.Account;
using Microsoft.AspNet.Identity;
using Services.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Innscoo.Infrastructure;

namespace Inscoo.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleManager;
        public UserController(IAppUserService appUserService, IAppRoleService appRoleManager)
        {
            _appRoleManager = appRoleManager;
            _appUserService = appUserService;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var list = _appUserService.GetUserList();
            var command = new PageCommand()
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                TotalPages = list.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(list);
        }
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            var roles = _appRoleManager.GetSelectList();
            var model = new RegisterViewModel() { selectList = roles };
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    LinkMan = model.Linkman,
                    CompanyName = model.CompanyName,
                    TiYong = model.TiYong,
                    FanBao = model.FanBao,
                    IsDelete = model.IsDelete,
                    CreaterId = User.Identity.GetUserId()
                };
                var result = _appUserService.CreateAsync(user, model.UserName, model.Password);
                if (result.Result.Succeeded)
                {
                    result = ForRole(user, model.Roles);
                    return View("Details", model);
                }
            }
            return View();
        }

        public Task<IdentityResult> ForRole(AppUser user, string roleName)
        {
            return _appUserService.AddToRoleAsync(user.Id, roleName);
        }
        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
