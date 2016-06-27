using Domain;
using Models;
using Microsoft.AspNet.Identity;
using Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using Innscoo.Infrastructure;
using System;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;

namespace Inscoo.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: User
        public ActionResult ListSearch()
        {
            var model = new Company();
            return View(model);
        }

        public ActionResult List()
        {
            //var roleId = Request.QueryString["roleId"];
            var companySearch = new vCompanySearch() { UserId = User.Identity.GetUserId() };
            var list = _companyService.GetCompanys(1, 20, companySearch);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(vCompanySearch companySearch)
        {
            var pageIndex = Request.QueryString["roleId"];
            var pageCount = Request.QueryString["userName"];
            var list = _companyService.GetCompanys(1, 20, companySearch);
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
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(vCompanyEdit model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            var model = _companyService.GetCompanyById(Convert.ToInt32(id));
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(vCompanyEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = _companyService.UpdateCompany(model);
                    if (result)
                        return RedirectToAction("Index");
                    else
                    {
                        throw new Exception("修改失败");
                    }
                }
                else
                {
                    throw new Exception("输入有误");
                }
            }
            catch
            {
                return View(model);
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
