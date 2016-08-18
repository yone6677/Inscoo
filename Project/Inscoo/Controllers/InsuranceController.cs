using System;
using Domain.Products;
using Models.Insurance;
using Models.Order;
using Models;
using Services;
using Services.Products;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Core.Pager;
using Domain;
using Innscoo.Infrastructure;
using Microsoft.AspNet.Identity;
using Models.Common;

namespace Inscoo.Controllers
{
    public class InsuranceController : BaseController
    {
        private readonly IMixProductService _mixProductService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;
        private readonly IArchiveService _archiveService;
        private readonly ICarInsuranceService _svCarInsuranceService;
        private readonly IPermissionService _svPermissionService;
        private readonly IAppUserService _appUserService;
        public InsuranceController(ICarInsuranceService svCarInsuranceService, IMixProductService mixProductService, IGenericAttributeService genericAttributeService, IProductService productService, IArchiveService archiveService, IPermissionService svPermissionService, IAppUserService appUserService)
        {
            _svCarInsuranceService = svCarInsuranceService;
            _mixProductService = mixProductService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
            _archiveService = archiveService;
            _svPermissionService = svPermissionService;
            _appUserService = appUserService;
        }
        // GET: Insurance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MixProduct()
        {
            var productSeries = _appUserService.GetProdSeries(User.Identity.GetUserId()).Select(p => p.Text).ToList();
            ViewBag.ProductSeries = productSeries;
            //var product = _mixProductService.GetAll();
            //var model = new List<RecommendationModel>();
            //foreach (var p in product)
            //{
            //    var item = new RecommendationModel()
            //    {
            //        Address = p.Address,
            //        AgeRange = p.AgeRange,
            //        Id = p.Id,
            //        Name = p.Name,
            //        Price = p.Price,
            //        StaffRange = p.StaffRange,

            //    };
            //    var itemModelList = new List<MixProductItemModel>();
            //    foreach (var s in p.ProductMixItem)
            //    {
            //        var itemModel = new MixProductItemModel()
            //        {
            //            CoverageSum = s.CoverageSum,
            //            Id = s.Id,
            //            mid = s.mid,
            //            OriginalPrice = s.OriginalPrice,
            //            SafefuardName = s.SafefuardName,
            //            ProdMemo = s.product.ProdMemo,
            //            ProdInsuredName = s.product.ProdInsuredName,
            //            ProvisionPath = s.product.ProvisionPath
            //        };
            //        itemModelList.Add(itemModel);
            //    }
            //    item.item = itemModelList;
            //    model.Add(item);
            //}
            //return PartialView(model);
            return PartialView();
        }
        public ActionResult CustomizeProduct()
        {
            var model = new CustomProductModel();
            model.CompanyList = new List<GenericAttributeModel>();
            var user = _appUserService.GetCurrentUser();
            if (user != null && !string.IsNullOrEmpty(user.ProdInsurance))//当前用户可选择的保险公司
            {
                var userProdIns = user.ProdInsurance.Split(new char[] { ';' });
                foreach (var p in userProdIns)
                {
                    if (!string.IsNullOrEmpty(p.Trim()))
                    {
                        var company = _genericAttributeService.GetByKey(null, "InsuranceCompany", p);
                        if (company != null)
                        {
                            var item = new GenericAttributeModel()
                            {
                                Key = company.Key,
                                Value = company.Value,
                                Sequence = company.Sequence
                            };
                            model.CompanyList.Add(item);
                        }
                    }
                }
            }
            model.Avarage = _genericAttributeService.GetSelectList("AgeRange");
            model.StaffsNumber = _genericAttributeService.GetSelectList("StaffRange");
            return View(model);
        }
        public ActionResult ProductList(string company = null, string productType = "员工福利保险", int stuffsNum = 1)
        {
            var model = new List<ProductListModel>();
            model = _productService.GetProductListForInscoo(company, productType, stuffsNum);
            return PartialView(model);
        }
        public ActionResult Cart(string company = null)
        {
            var model = new CustomizeBuyModel()
            {
                companyName = company,
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GetProductPrice(int cid = 0, string payrat = null, int staffsNumber = 0, int avarage = 0)
        {
            if (cid > 0)
            {
                var model = _productService.GetProductPrice(cid, payrat, staffsNumber, avarage);
                if (model != null)
                {
                    return Json(model);
                }
            }
            return null;
        }

        #region 车险
        public ActionResult CarInscuranceSearch(int fileType = 0)
        {
            //ViewBag.RoleId = _appUserService.GetRolesManagerPermissionByUserId(User.Identity.GetUserId(), "Id");
            var roles = _appUserService.GetRolesByUserId(User.Identity.GetUserId());
            ViewBag.CanCreate = roles.Contains("CarInscuranceCustomer");
            ViewBag.FileType = fileType;
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult CarInscuranceList(int fileType, int pageIndex = 1, int pageSize = 15)
        {
            var uId = User.Identity.GetUserId();
            var roles = _appUserService.GetRolesByUserId(User.Identity.GetUserId());
            var canEdit = roles.Contains("CarInscuranceCustomer");
            ViewBag.CanEdit = canEdit;
            ViewBag.FileType = fileType;
            if (!canEdit) uId = "-1";//车险用户可以编辑，只能查看自己上传的文件。车险公司不能编辑，但可以查看所有。

            //出admin外，其他用户只能看到自己创建的用户
            IPagedList<vCarInsuranceList> list = _archiveService.GetCarInsuranceExcel(fileType, pageIndex, pageSize, uId);

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
        [AllowAnonymous]
        public ActionResult CarInscuranceDetailSearch()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult CarInscuranceDetailList()
        {
            var model = new CarInsuranceDetailSearchModel();
            IPagedList<CarInsuranceDetail> list = _svCarInsuranceService.GetDetails(model, 1, 15);

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
        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult CarInscuranceDetailList(CarInsuranceDetailSearchModel model, int pageIndex = 1, int pageSize = 15)
        {

            IPagedList<CarInsuranceDetail> list = _svCarInsuranceService.GetDetails(model, pageIndex, pageSize);

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
        public ActionResult CarInscuranceCreate(string excelId, int fileType = 0)
        {
            ViewBag.ExcelId = excelId;
            ViewBag.FileType = fileType;
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarInscuranceCreate(HttpPostedFileBase excel, string excelId, int fileType)
        {
            try
            {
                ViewBag.ExcelId = excelId;
                ViewBag.FileType = fileType;
                var uId = User.Identity.GetUserId();
                var uName = User.Identity.Name;
                string mailContent, path;
                if (fileType == 0)
                {
                    if (string.IsNullOrEmpty(excelId))
                    {
                        path = _archiveService.InsertCarInsuranceExcel(excel, uId,
                           uName, fileType);
                        mailContent = string.Format("用户：{0}上传车险{1}", uName, excel.FileName);
                    }
                    else
                    {
                        mailContent = string.Format("用户：{0}重新上传车险{1}", uName, excel.FileName);
                        path = _archiveService.UpdateCarInsuranceExcel(excel, excelId, uName);
                    }
                    var mailTo = _genericAttributeService.GetByGroup("CarInscuranceMailTo").Select(c => c.Value);
                    MailService.SendMailAsync(new MailQueue()
                    {
                        MQTYPE = "UploadCarInscurance",
                        MQSUBJECT = "上传车险通知",
                        MQMAILCONTENT = "",
                        MQMAILFRM = "service@inscoo.com",
                        MQMAILTO = string.Join(";", mailTo),
                        MQFILE = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1)
                    });
                }
                else if (fileType == 1)
                {
                    if (string.IsNullOrEmpty(excelId))
                    {
                        path = _archiveService.InsertCarInsuranceExcel(excel, uId,
                           uName, fileType);
                    }
                    else
                    {
                        path = _archiveService.UpdateCarInsuranceExcel(excel, excelId, uName);
                    }

                }
                ViewBag.Mes = "上传成功";
            }
            catch (Exception e)
            {
                ViewBag.Mes = "上传失败";
            }
            return View();
        }
        public ActionResult CarInscuranceUploadEinsurance(int insuranceId, string uKey)
        {
            ViewBag.InsuranceId = insuranceId;
            ViewBag.UKey = uKey;
            var carInsurance = _archiveService.GetCarEInsuranceUrl(insuranceId, uKey);
            if (carInsurance.Einsurance != null)
                ViewBag.Url = carInsurance.Einsurance.Url;
            if (carInsurance.EOrderCode != null)
                ViewBag.EOrderCode = carInsurance.EOrderCode;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarInscuranceUploadEinsurance(HttpPostedFileBase excel, int insuranceId, string uKey, string code)
        {
            try
            {
                ViewBag.InsuranceId = insuranceId;
                ViewBag.UKey = uKey;
                var userName = User.Identity.Name;
                string mailContent, path;
                path = _archiveService.InsertCarInsuranceEinsurance(excel, userName, insuranceId, uKey, code);

                if (path != null)
                {
                    mailContent = string.Format("用户：{0}上传车险电子保单{1}", userName, excel.FileName);
                    var mailTo = _genericAttributeService.GetByGroup("CarCustomerMailTo").Select(c => c.Value);
                    MailService.SendMailAsync(new MailQueue()
                    {
                        MQTYPE = "UploadCarInscurance",
                        MQSUBJECT = "上传车险电子保单通知",
                        MQMAILCONTENT = "",
                        MQMAILFRM = "service@inscoo.com",
                        MQMAILTO = string.Join(";", mailTo),
                        MQFILE = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1)

                    });
                }
                ViewBag.Mes = "操作成功";
            }
            catch (WarningException e)
            {
                ViewBag.Mes = e.Message;
            }
            catch (Exception e)
            {
                ViewBag.Mes = "操作失败";
            }
            return RedirectToAction("CarInscuranceSearch");
        }

        public ActionResult CarInscuranceAddEOrderCode(string insuranceId, string uKey)
        {
            ViewBag.InsuranceId = insuranceId;
            ViewBag.UKey = uKey;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarInscuranceAddEOrderCode(int insuranceId, string uKey, string code)
        {
            try
            {
                ViewBag.InsuranceId = insuranceId;
                ViewBag.UKey = uKey;
                _archiveService.UploadCarInsuranceEOrderCode(code, insuranceId, uKey);

                ViewBag.Mes = "操作成功";
            }
            catch (WarningException e)
            {
                ViewBag.Mes = e.Message;
            }
            catch (Exception e)
            {
                ViewBag.Mes = "操作失败";
            }
            return View();
        }
        public ActionResult CarInscuranceDelete(int insuranceId)
        {
            _archiveService.DeleteCarInsuranceExcel(insuranceId);
            return RedirectToAction("CarInscuranceSearch");
        }
        #endregion
        #region 会员
        public ActionResult MemberInscuranceSearch(int fileType)
        {
            ViewBag.FileType = fileType;
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult MemberInscuranceList(int fileType, int pageIndex = 1, int pageSize = 15)
        {
            var author = User.Identity.Name;
            //var roles = _appUserService.GetRolesByUserId(User.Identity.GetUserId());
            var canEdit = User.IsInRole("MemberCard");
            ViewBag.isMemberCard = canEdit;
            ViewBag.FileType = fileType;
            if (!canEdit) author = "-1";//车险用户可以编辑，只能查看自己上传的文件。车险公司不能编辑，但可以查看所有。

            //出admin外，其他用户只能看到自己创建的用户
            IPagedList<vMemberInsuranceList> list = _archiveService.GetMemberInsuranceExcel(fileType, pageIndex, pageSize, author);

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
        /*[AllowAnonymous]
        public ActionResult CarInscuranceDetailSearch()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult CarInscuranceDetailList()
        {
            var model = new CarInsuranceDetailSearchModel();
            IPagedList<CarInsuranceDetail> list = _svCarInsuranceService.GetDetails(model, 1, 15);

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
        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult CarInscuranceDetailList(CarInsuranceDetailSearchModel model, int pageIndex = 1, int pageSize = 15)
        {

            IPagedList<CarInsuranceDetail> list = _svCarInsuranceService.GetDetails(model, pageIndex, pageSize);

            var command = new PageCommand()
            {
                PageIndex = list.PageIndex,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount,
                TotalPages = list.TotalPages
            };
            ViewBag.pageCommand = command;

            return PartialView(list);
        }*/
        public ActionResult MemberInscuranceCreate(string excelId, string fileTypeName, int fileType = 0)
        {
            ViewBag.ExcelId = excelId;
            ViewBag.FileType = fileType;
            ViewBag.FileTypeName = fileTypeName;
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberInscuranceCreate(HttpPostedFileBase excel, string excelId, int fileType, string fileTypeName)
        {
            try
            {
                ViewBag.ExcelId = excelId;
                ViewBag.FileType = fileType;
                var uId = User.Identity.GetUserId();
                var uName = User.Identity.Name;
                string mailContent, path;

                if (string.IsNullOrEmpty(excelId))
                {
                    path = _archiveService.InsertMemberInsuranceExcel(excel,
                       uName, fileTypeName, fileType);
                    //mailContent = string.Format("用户：{0}上传车险{1}", uName, excel.FileName);
                }
                else
                {
                    //mailContent = string.Format("用户：{0}重新上传车险{1}", uName, excel.FileName);
                    path = _archiveService.UpdateCarInsuranceExcel(excel, excelId, uName);
                }
                //var mailTo = _genericAttributeService.GetByGroup("CarInscuranceMailTo").Select(c => c.Value);
                //MailService.SendMailAsync(new MailQueue()
                //{
                //    MQTYPE = "UploadCarInscurance",
                //    MQSUBJECT = "上传车险通知",
                //    MQMAILCONTENT = "",
                //    MQMAILFRM = "service@inscoo.com",
                //    MQMAILTO = string.Join(";", mailTo),
                //    MQFILE = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1)
                //});

                ViewBag.Mes = "上传成功";
            }
            catch (Exception e)
            {
                ViewBag.Mes = "上传失败";
            }
            return View();
        }
        public ActionResult MemberInscuranceUploadEinsurance(int insuranceId, string uKey)
        {
            ViewBag.InsuranceId = insuranceId;
            ViewBag.UKey = uKey;
            var insurance = _archiveService.GetMemberEInsuranceUrl(insuranceId, uKey);
            if (insurance.Einsurance != null)
                ViewBag.Url = insurance.Einsurance.Url;
            if (insurance.EOrderCode != null)
                ViewBag.EOrderCode = insurance.EOrderCode;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberInscuranceUploadEinsurance(HttpPostedFileBase excel, int insuranceId, string uKey, string code)
        {
            try
            {
                ViewBag.InsuranceId = insuranceId;
                ViewBag.UKey = uKey;
                var userName = User.Identity.Name;
                string mailContent, path;
                path = _archiveService.InsertMemberInsuranceEinsurance(excel, userName, insuranceId, uKey, code);

                //if (path != null)
                //{
                //    mailContent = string.Format("用户：{0}上传车险电子保单{1}", userName, excel.FileName);
                //    var mailTo = _genericAttributeService.GetByGroup("CarCustomerMailTo").Select(c => c.Value);
                //    MailService.SendMailAsync(new MailQueue()
                //    {
                //        MQTYPE = "UploadCarInscurance",
                //        MQSUBJECT = "上传车险电子保单通知",
                //        MQMAILCONTENT = "",
                //        MQMAILFRM = "service@inscoo.com",
                //        MQMAILTO = string.Join(";", mailTo),
                //        MQFILE = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1)

                //    });
                //}
                ViewBag.Mes = "操作成功";
            }
            catch (WarningException e)
            {
                ViewBag.Mes = e.Message;
            }
            catch (Exception e)
            {
                ViewBag.Mes = "操作失败";
            }
            return RedirectToAction("CarInscuranceSearch");
        }

        public ActionResult MemberInscuranceAddEOrderCode(string insuranceId, string uKey)
        {
            ViewBag.InsuranceId = insuranceId;
            ViewBag.UKey = uKey;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberInscuranceAddEOrderCode(int insuranceId, string uKey, string code)
        {
            try
            {
                ViewBag.InsuranceId = insuranceId;
                ViewBag.UKey = uKey;
                _archiveService.UploadMemberInsuranceEOrderCode(code, insuranceId, uKey);

                ViewBag.Mes = "操作成功";
            }
            catch (WarningException e)
            {
                ViewBag.Mes = e.Message;
            }
            catch (Exception e)
            {
                ViewBag.Mes = "操作失败";
            }
            return View();
        }
        public ActionResult MemberInscuranceDelete(int insuranceId)
        {
            _archiveService.DeleteMemberInsuranceExcel(insuranceId);
            return RedirectToAction("MemberInscuranceSearch");
        }
        #endregion
    }
}