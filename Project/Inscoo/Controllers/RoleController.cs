using Core;
using Innscoo.Infrastructure;
using Inscoo.Models.Role;
using Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IAppRoleService _appRoleService;
        public RoleController(IAppRoleService appRoleService)
        {
            _appRoleService = appRoleService;
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var roles = _appRoleService.Roles();
            var model = new List<RoleModel>();
            foreach (var s in roles)
            {
                var item = new RoleModel()
                {
                    Name = s.Name,
                    Description = s.Description
                };
                model.Add(item);
            }
            var command = new PageCommand();
            ViewBag.pageCommand = command;
            return PartialView(model);
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new AppRole()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                var result = await _appRoleService.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
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

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
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
