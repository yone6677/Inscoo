﻿using Domain;
using Models;
using Models.User;
using Microsoft.AspNet.Identity;
using Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using Innscoo.Infrastructure;
using System;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Pager;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Inscoo.Controllers
{
    public class ClaimController : BaseController
    {
        private readonly IClaimService _svClaim;
        public ClaimController(IClaimService svClaim)
        {
            _svClaim = svClaim;
        }
        // GET: User
        public ActionResult ListSearch()
        {
            var model = new vClaimManagementDetailListSearch();
            model.ClaimAccdtDateBegin = DateTime.Now.AddMonths(-1).Date;
            model.ClaimAccdtDateEnd = DateTime.Now.Date;
            return View(model);
        }

        public PartialViewResult ListData(vClaimManagementDetailListSearch model, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                var list = _svClaim.GetClaimsDetailList(pageIndex, pageSize, model);


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
            catch (Exception)
            {
                return PartialView();
            }
        }
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

    }
}