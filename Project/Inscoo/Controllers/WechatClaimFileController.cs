using Domain;
using Innscoo.Infrastructure;
using Models.Claim;
using Services;
using Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class WechatClaimFileController : BaseController
    {
        private readonly IClaimAPiService _claimService;
        private readonly IClaimFileApiService _claimFileService;
        private readonly IArchiveService _archiveService;
        public WechatClaimFileController(IClaimAPiService claimService, IClaimFileApiService claimFileService)
        {
            _claimService = claimService;
            _claimFileService = claimFileService;
        }
        // GET: WechatClaimFile
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult List(int pageIndex = 1, int pageSize = 15, DateTime? beginDate = null, DateTime? endDate = null, int state = 0)
        {
            var model = _claimService.GetListOfPager(pageIndex, pageSize, beginDate, endDate, state);
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
        public ActionResult File(int id)
        {
            var claim = _claimService.GetById(id);
            var claimFile = _claimFileService.GetByCId(id);
            var model = new List<WechatFileModel>();
            if (claimFile.Any())
            {
                foreach (var c in claimFile)
                {
                    var file = _archiveService.GetById(c.FileId);
                    if (file != null)
                    {
                        var item = new WechatFileModel()
                        {
                            FType = c.fileType.ToString(),
                            Url = file.Url,
                            Id = file.Id
                        };
                        model.Add(item);
                    }
                }
            }
            ViewBag.Id = claim.Id;
            ViewBag.State = claim.State;
            return View(model);
        }
        public ActionResult Audit(int id, int Status)
        {
            var claim = _claimService.GetById(id);
            claim.State = Status;
            _claimService.Update(claim);
            return RedirectToAction("Index");
        }
    }
}