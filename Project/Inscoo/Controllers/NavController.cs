﻿using Domain.Navigation;
using Innscoo.Infrastructure;
using Models.Navigation;
using Services.Navigation;
using System.Collections.Generic;
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
        public ActionResult Menu()
        {
            return PartialView();
        }
        // GET: Nav
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(int pid = 0)
        {
            var model = _navService.GetList(1, 15, null, pid);
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
        public ActionResult List(int PageIndex = 1, int PageSize = 15, int pid = 0)
        {
            var model = _navService.GetList(PageIndex, PageSize, null, pid);
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
                    var model = new NavigationViewModel()
                    {
                        Id = item.Id,
                        action = item.action,
                        controller = item.controller,
                        isShow = item.isShow,
                        level = item.level,
                        memo = item.memo,
                        name = item.name,
                        pId = item.pId,
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
        public ActionResult Create(int id = 0, int level = 0)
        {
            var model = new NavigationViewModel();
            model.pId = id;
            model.level = level;
            return View(model);
        }

        // POST: Nav/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NavigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new NavigationItem();
                if (!string.IsNullOrEmpty(item.action))
                {
                    item.action = model.action.Trim().ToLower();
                }
                if (!string.IsNullOrEmpty(item.controller))
                {
                    item.controller = model.controller.Trim().ToLower();
                }
                item.isShow = model.isShow;
                item.level = model.level;
                item.memo = model.memo;
                item.name = model.name;
                item.pId = model.pId;
                item.isShow = model.isShow;
                item.url = model.controller + "/" + model.action;
                item.htmlAtt = model.htmlAtt;
                item.sequence = model.sequence;
                if (_navService.Insert(item))
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
            var model = new NavigationViewModel()
            {
                Id = item.Id,
                action = item.action,
                controller = item.controller,
                isShow = item.isShow,
                memo = item.memo,
                name = item.name,
                sequence = item.sequence,
                htmlAtt = item.htmlAtt
            };
            return View(model);
        }

        // POST: Nav/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NavigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = _navService.GetById(model.Id);
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.action))
                    {
                        item.action = model.action.Trim().ToLower();
                    }
                    if (!string.IsNullOrEmpty(item.controller))
                    {
                        item.controller = model.controller.Trim().ToLower();
                    }
                    item.isShow = model.isShow;
                    item.memo = model.memo;
                    item.name = model.name;
                    item.isShow = model.isShow;
                    item.htmlAtt = model.htmlAtt;
                    item.sequence = model.sequence;
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
                    var model = new NavigationViewModel()
                    {
                        Id = item.Id,
                        action = item.action,
                        controller = item.controller,
                        isShow = item.isShow,
                        level = item.level,
                        memo = item.memo,
                        name = item.name,
                        pId = item.pId,
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(NavigationViewModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    _navService.DeleteById(model.Id);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

    }
}
