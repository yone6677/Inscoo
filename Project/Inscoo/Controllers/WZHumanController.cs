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
using System.Configuration;
using System.ComponentModel;
using System.Linq;

namespace Inscoo.Controllers
{
    public class WZHumanController : BaseController
    {
        private readonly IWZHumanService _svWZHuman;
        private readonly IArchiveService _svArchive;
        private readonly IAppUserService _svUser;
        private readonly IGenericAttributeService _svGenericAttribute;
        public WZHumanController(IGenericAttributeService svGenericAttribute, IAppUserService svUser, IWZHumanService svWZHuman, IArchiveService svArchive)
        {
            _svGenericAttribute = svGenericAttribute;
            _svUser = svUser;
            _svWZHuman = svWZHuman;
            _svArchive = svArchive;
        }
        // GET: User
        public ActionResult ListIndex(int pageIndex = 1)
        {
            var model = new WZSearchModel();
            model.UserId = User.Identity.GetUserId();
            ViewBag.pageIndex = pageIndex;
            return View(model);
        }

        public ActionResult ListData(int pageIndex = 1)
        {

            var search = new WZSearchModel() { Author = User.Identity.Name };
            var list = _svWZHuman.GetWZList(search, pageIndex);
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
        public ActionResult ListData(WZSearchModel search, int pageIndex = 1, int pageSize = 15)
        {
            search.Author = User.Identity.Name;
            var list = _svWZHuman.GetWZList(search, pageIndex: pageIndex, pageSize: pageSize);
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
        public ActionResult Create(int pageIndex = 1)
        {
            ViewBag.pageIndex = pageIndex;
            var model = new WZCreateModel();
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WZCreateModel model, int pageIndex = 1)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var uId = User.Identity.GetUserId();
                    var user = new AppUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        CompanyName = model.CompanyName,
                        IsDelete = false,
                        CreaterId = uId,
                        Changer = uId,
                        ProdSeries = "WZHumanResource",
                    };
                    var defaultPwd = ConfigurationManager.AppSettings["newPwd"];
                    var result = await _svUser.CreateAsync(user, "", defaultPwd);
                    if (result.Succeeded)
                    {
                        _svWZHuman.AddNewWZHum2anMaster(new WZHumanMaster()
                        {
                            CompanyName = user.CompanyName,
                            Account = user.UserName,
                            Author = User.Identity.Name
                        });

                        if (ForRole(user, "WZHumanCustomer"))
                        {
                            return RedirectToAction("ListIndex", new { pageIndex });
                        }
                        else
                        {
                            TempData["errorMes"] = "添加失败";
                        }
                    }
                }
            }
            catch (WarningException we)
            {
                TempData["errorMes"] = we.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMes"] = "添加失败";
            }
            ViewBag.pageIndex = pageIndex;
            return View(model);
        }

        public ActionResult FileListIndex(int masterId)
        {
            //判断是否有权限
            if (!_svWZHuman.HasPerminsion(masterId, User.Identity.Name)) return RedirectToAction("ListIndex");
            ViewBag.MasterId = masterId;
            return View();
        }

        public ActionResult FileListData(int masterId)
        {
            var search = new WZFileSearchModel() { MasterId = masterId };
            var list = _svArchive.GetWZFileDataModels(search, 1, 15);
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
        public ActionResult FileListData(WZFileSearchModel search, int pageIndex = 1, int pageSize = 15)
        {
            var list = _svArchive.GetWZFileDataModels(search, pageIndex, pageSize);
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
        public ActionResult UploadInsurants(HttpPostedFileBase excel, int masterId, string memo = "")
        {
            try
            {
                var userName = User.Identity.Name;
                string mailContent, path;
                path = _svArchive.InsertWZInsurants(excel, userName, masterId, memo);

                if (path != null)
                {
                    mailContent = string.Format("用户：{0}上传车险电子保单{1}", userName, excel.FileName);

                    var isFirstUpload = !_svArchive.GetByTypeAndPId(masterId, "WZHuman").Any();
                    var mailTo = isFirstUpload ? _svGenericAttribute.GetByGroup("WZHumanEmail").Select(c => c.Value) : _svGenericAttribute.GetByGroup("WZHumanEmailMaintain").Select(c => c.Value);
                    MailService.SendMailAsync(new MailQueue()
                    {
                        MQTYPE = "UploadWZHuman",
                        MQSUBJECT = "上传保险人员名单",
                        MQMAILCONTENT = "",
                        MQMAILFRM = "service@inscoo.com",
                        MQMAILTO = string.Join(";", mailTo),
                        MQFILE = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1)
                    });
                    TempData["errorMes"] = "上传成功";
                }
                else
                {
                    TempData["errorMes"] = "上传失败";
                }
            }
            catch (WarningException e)
            {
                TempData["errorMes"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["errorMes"] = "上传失败";
            }
            return RedirectToAction("FileListIndex", new { masterId });
        }
        bool ForRole(AppUser user, string roleName)
        {
            return _svUser.DeleteBeforeRoleAndNew(user.Id, roleName);
        }
    }
}
