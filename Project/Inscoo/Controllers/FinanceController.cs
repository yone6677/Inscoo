using Innscoo.Infrastructure;
using Services.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class FinanceController : BaseController
    {
        private readonly ICashFlowDetailsService _cashFlowDetails;
        private readonly ICashFlowService _cashFlow;

        public FinanceController(ICashFlowDetailsService cashFlowDetails, ICashFlowService cashFlow)
        {
            _cashFlow = cashFlow;
            _cashFlowDetails = cashFlowDetails;
        }
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var model = _cashFlow.GetListOfPager(1, 15);
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
        public ActionResult List(int PageIndex = 1, int PageSize = 15, int oId = 0, DateTime? beginDate = null, DateTime? endDate = null)
        {
            var model = _cashFlow.GetListOfPager(PageIndex, PageSize, 0, oId, beginDate, endDate);
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
        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult CashFlowDetails(int cid)
        {
            var model = _cashFlowDetails.GetListOfPager(1, 15, cid);
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
        public ActionResult CashFlowDetails(int cid, int PageIndex = 1, int PageSize = 15)
        {
            var model = _cashFlowDetails.GetListOfPager(PageIndex, PageSize, cid);
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
    }
}