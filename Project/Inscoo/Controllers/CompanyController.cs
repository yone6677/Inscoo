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
using System.Web;

namespace Inscoo.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IArchiveService _archiveService;
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService, IArchiveService archiveService)
        {
            _companyService = companyService;
            _archiveService = archiveService;
        }
        // GET: User
        public ActionResult ListIndex()
        {
            var model = new vCompanySearch();
            model.UserId = User.Identity.GetUserId();
            return View(model);
        }

        public ActionResult ListData()
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
        public ActionResult ListData(vCompanySearch companySearch)
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
            var model = new vCompanyAdd();
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vCompanyAdd model, HttpPostedFileBase BusinessLicense)
        {
            if (ModelState.IsValid)
            {
                var context = HttpContext;
                var comId = _companyService.AddNewCompany(model, User.Identity.GetUserId());
                _archiveService.InsertBusinessLicense(BusinessLicense, User.Identity.Name, comId);
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
        public ActionResult Edit(vCompanyEdit model, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(file.FileName))
                        _archiveService.InsertBusinessLicense(file, User.Identity.Name, model.Id);
                    var result = _companyService.UpdateCompany(model);
                    if (result)
                        return RedirectToAction("ListIndex");
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
            _companyService.DeletetById(id);
            return RedirectToAction("ListIndex");
        }

    }
}
