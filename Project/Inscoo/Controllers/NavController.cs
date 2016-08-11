using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Innscoo.Infrastructure;
using Models.Navigation;
using Services;
using System.Net;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class NavController : BaseController
    {
        private readonly INavigationService _navService;
        public NavController(INavigationService navService)
        {
            _navService = navService;
        }
        // GET: Nav
        public ActionResult Index()
        {
            ViewBag.ParentList = _navService.GetParentNavList();
            return View();
        }
        public ActionResult List(int pid = 0)
        {
            var model = _navService.GetList(1, 15, pid);
            var command = new PageCommand()
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                TotalCount = model.TotalCount,
                TotalPages = model.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(int pageIndex = 1, int pageSize = 15, int pid = 0)
        {
            var r = Request;
            var model = _navService.GetList(pageIndex, pageSize, pid);
            var command = new PageCommand()
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                TotalCount = model.TotalCount,
                TotalPages = model.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(model);
        }
        // GET: Nav/Details/5
        public ActionResult Details(int id)
        {
            if (id > 0)
            {
                var item = _navService.GetById(id);
                if (item != null)
                {
                    var model = new NavigationModel()
                    {
                        Id = item.Id,
                        action = item.action,
                        controller = item.controller,
                        isShow = item.isShow,
                        level = item.level,
                        memo = item.memo,
                        name = item.name,
                        PId = item.pId,
                        url = item.url,
                        htmlAtt = item.htmlAtt,
                        SonMenu = _navService.GetSonViewList(id)
                    };
                    return View(model);
                }
            }
            return View("Index");
        }

        // GET: Nav/Create
        public ActionResult Create()
        {
            ViewBag.ParentList = _navService.GetParentNavList();
            ViewBag.Controllers = GetAllControles();
            var model = new NavigationModel();
            return View(model);
        }

        // POST: Nav/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NavigationModel model)
        {

            if (ModelState.IsValid)
            {
                var isExist = false;

                var item = _navService.GetByUrl(model.controller, model.action);

                if (!string.IsNullOrEmpty(model.controller))//非一级菜单
                {
                    if (item != null)
                    {
                        isExist = true;
                        item = _navService.GetById(item.Id);
                    }
                }
                if (item == null)
                    item = new Navigation();
                item.action = model.action ?? "";
                item.controller = model.controller ?? "";
                item.level = model.PId == 0 ? 0 : 1;
                item.memo = model.memo;
                item.name = model.name;
                item.pId = model.PId;
                item.isShow = true;
                item.IsDeleted = false;
                item.url = item.controller.Replace("Controller", "") + "/" + item.action;
                item.htmlAtt = model.htmlAtt;
                item.sequence = model.sequence;
                if (isExist)
                {
                    if (_navService.Update(item)) return RedirectToAction("Index");
                }
                else if (_navService.Insert(item))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Nav/Edit/5
        public ActionResult Edit(int id)
        {

            var item = _navService.GetById(id);
            var model = new NavigationModel()
            {
                PId = item.pId,
                Id = item.Id,
                action = item.action ?? "",
                controller = item.controller ?? "",
                isShow = item.isShow,
                memo = item.memo,
                name = item.name,
                sequence = item.sequence,
                htmlAtt = item.htmlAtt
            };
            ViewBag.ParentList = _navService.GetParentNavList(model.PId);
            ViewBag.Controllers = GetAllControles(model.controller);
            return View(model);
        }

        // POST: Nav/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NavigationModel model)
        {
            if (ModelState.IsValid)
            {
                var item = _navService.GetById(model.Id);
                if (item != null)
                {
                    item.pId = model.PId;
                    item.action = model.action ?? "";
                    item.controller = model.controller ?? "";
                    item.url = item.controller.Replace("Controller", "") + "/" + item.action;
                    item.isShow = model.isShow;
                    item.memo = model.memo;
                    item.name = model.name;
                    item.isShow = model.isShow;
                    item.htmlAtt = model.htmlAtt;
                    item.sequence = model.sequence;
                    item.level = model.PId == 0 ? 0 : 1;
                    if (_navService.Update(item))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }

        // GET: Nav/Delete/5
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var item = _navService.GetById(id);
                if (item != null)
                {
                    var model = new NavigationModel()
                    {
                        Id = item.Id,
                        action = item.action,
                        controller = item.controller,
                        isShow = item.isShow,
                        level = item.level,
                        memo = item.memo,
                        name = item.name,
                        PId = item.pId,
                        url = item.url,
                        htmlAtt = item.htmlAtt,
                        SonMenu = _navService.GetSonViewList(id)
                    };
                    return View(model);
                }
            }
            return View("Index");
        }

        // POST: Nav/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_navService.DeleteById(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index");
        }
        SelectList GetAllControles(string selectedValue = "")
        {
            var controllers = typeof(BaseController).Assembly.GetTypes().Where(t => t.BaseType.Name.Contains("BaseController")).Select(c => new { Text = c.Name, Value = c.Name }).ToList();
            controllers.Add(new { Text = "无", Value = "" });
            return new SelectList(controllers, "Value", "Text", selectedValue);
        }
    }
}
