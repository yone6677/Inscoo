using Domain.Finance;
using Innscoo.Infrastructure;
using Models.Finance;
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
        public ActionResult Settlement(int id)
        {
            var model = new CashFlowDetailsModel();
            model.cId = id;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settlement(CashFlowDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new CashFlowDetails()
                {
                    ActualCollected = model.ActualCollected,
                    Author = User.Identity.Name,
                    cId = model.cId,
                    Memo = model.Memo,
                    RealPayment = model.RealPayment,
                    TransferVoucher = model.TransferVoucher
                };
                if (_cashFlowDetails.Insert(item))
                {
                    var cashFlow = _cashFlow.GetById(model.cId);
                    if (cashFlow != null)
                    {
                        cashFlow.Difference += model.ActualCollected + model.RealPayment;
                        _cashFlow.Update(cashFlow);
                    }
                }
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }
        public ActionResult CashFlowDtl(int id)
        {
            var item = _cashFlowDetails.GetById(id);
            var model = new CashFlowDetailsModel();
            if (item != null)
            {
                model.ActualCollected = item.ActualCollected;
                model.Author = item.Author;
                model.cId = item.cId;
                model.CreateTime = item.CreateTime;
                model.Id = item.Id;
                model.Memo = item.Memo;
                model.Payable = item.Payable;
                model.RealPayment = item.RealPayment;
                model.Receivable = item.Receivable;
                model.TransferVoucher = item.TransferVoucher;
            }
            return View(model);
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