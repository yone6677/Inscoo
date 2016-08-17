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
using System.Collections.Generic;

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
                            var mailContent = string.Format("<p><b>{0}</b>,您好：</p><div style=\"text-indent:4em;\"><p>已为您开通保酷平台的用户权限，请登录使用，详情如下：</p><p>            登录网站：<b>www.inscoo.com</b></p><p>            登录账号：<b>{1}</b></p><p>            密码：<b>inscoo</b></p><p>请您在首次登录后立即修改密码，谢谢！</p><br><p>如果有任何疑问，请随时拨打400-612-6750咨询！</p><p>欢迎加入保酷大家庭，祝您工作愉快，顺祝商祺！</p><br></div><p><b>保酷网 www.inscoo.com</b></p><p style=\"overflow:hidden\"><img src=\"http://www.inscoo.com/Content/img/InscooLogo.png\"alt=\"\"style=\"float: left;\" /><img src=\"http://www.inscoo.com/Content/img/InscooWeChat.png\" alt=\"\" style=\"float: left;\" /></p><p>上海皓为商务咨询有限公司</p>", user.UserName, user.Email);
                            MailService.SendMail(new MailQueue()
                            {
                                MQTYPE = "保酷账号",
                                MQSUBJECT = "保酷账号",
                                MQMAILCONTENT = mailContent,
                                MQMAILFRM = "service@inscoo.com",
                                MQMAILTO = user.Email,
                                //MQFILE = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\caozuozhinan.docx"

                            });
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
            ViewBag.TypeList = new List<SelectListItem>()
            {
                new SelectListItem() { Value="0",Text="选择类型" },
                new SelectListItem() { Value="1",Text="投保人员名单上传" },
                new SelectListItem() { Value="2",Text="退保人员名单上传"},
                new SelectListItem() { Value="3",Text="报案人员名单上传" }
            };
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
        public ActionResult UploadInsurants(HttpPostedFileBase excel, int masterId, int typeList, string memo = "")
        {
            try
            {
                string typeDes = "";
                switch (typeList)
                {
                    case 0: throw new WarningException("请选择类型");
                    case 1: typeDes = "投保"; break;
                    case 2: typeDes = "退保"; break;
                    case 3: typeDes = "报案"; break;
                    default: typeDes = "WZHuman"; break;
                }
                var isFirstUpload = !_svArchive.GetByTypeAndPId(masterId, "WZHuman").Any();
                var userName = User.Identity.Name;
                string mailContent, path;
                path = _svArchive.InsertWZInsurants(excel, userName, masterId, memo, typeDes);

                if (path != null)
                {
                    mailContent = string.Format("用户：{0}上传车险电子保单{1}", userName, excel.FileName);

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

        public ActionResult DeleteFile(int id, int masterId)
        {
            try
            {
                var result = _svArchive.DeleteById(id, true, User.Identity.Name);
                if (result)
                {
                    TempData["errorMes"] = "删除成功";
                }
                else
                {
                    TempData["errorMes"] = "删除失败";
                }
            }
            catch (Exception)
            {
                TempData["errorMes"] = "删除失败";
            }
            return RedirectToAction("FileListIndex", new { masterId });
        }
        bool ForRole(AppUser user, string roleName)
        {
            return _svUser.DeleteBeforeRoleAndNew(user.Id, roleName);
        }
    }
}
