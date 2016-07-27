using Domain.Claim;
using Domain.Orders;
using Models.Api.Archive;
using Models.Api.Claim;
using Models.Infrastructure;
using Services;
using Services.Api;
using Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Inscoo.Controllers
{
    public class ClaimApiController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderEmpService _orderEmpService;
        private readonly IClaimAPiService _claimApiService;
        private readonly ILoggerService _loggerService;
        private readonly IArchiveService _archiveService;
        private readonly IClaimFileApiService _clamFileAPiService;
        public ClaimApiController(IOrderService orderService, IOrderEmpService orderEmpService, IClaimAPiService claimApiService,
            ILoggerService loggerService, IArchiveService archiveService, IClaimFileApiService clamFileAPiService)
        {
            _orderService = orderService;
            _orderEmpService = orderEmpService;
            _claimApiService = claimApiService;
            _loggerService = loggerService;
            _archiveService = archiveService;
            _clamFileAPiService = clamFileAPiService;
        }
        [ResponseType(typeof(ClaimDetailsApi))]
        public IHttpActionResult GetById(int id)
        {
            var item = _claimApiService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var invoiceNum = _clamFileAPiService.GetByCId(id, 1).Count();
            var caseNum = _clamFileAPiService.GetByCId(id, 2).Count();
            var otherNum = _clamFileAPiService.GetByCId(id, 3).Count();
            var model = new ClaimDetailsApi()
            {
                CaseId = item.CaseId,
                CaseNum = caseNum,
                CompanyName = item.CompanyName,
                Describe = item.Describe,
                Id = item.Id,
                InvoiceNum = invoiceNum,
                OtherNum = otherNum,
                ProposerBirthday = item.ProposerBirthday,
                ProposerEmail = item.ProposerEmail,
                ProposerIdNumber = item.ProposerIdNumber,
                ProposerIdType = item.ProposerIdType,
                ProposerName = item.ProposerName,
                ProposerPhone = item.ProposerPhone,
                ProposerSex = item.ProposerSex,
                RecipientBirthday = item.RecipientBirthday,
                RecipientEmail = item.RecipientEmail,
                RecipientIdNumber = item.RecipientIdNumber,
                RecipientIdType = item.RecipientIdType,
                RecipientName = item.RecipientName,
                RecipientPhone = item.RecipientPhone,
                RecipientSex = item.RecipientSex
            };
            return Ok(model);
        }

        [HttpPost]
        public int PostClaim(ClaimModel model)
        {
            try
            {
                var item = new ClaimFromWechatItem();
                item.Author = null;
                item.CaseId = DateTime.Now.ToString("yyMMdd") + DateTime.Now.Ticks;
                var empList = _orderEmpService.GetByInfo(model.IdNumer.Trim(), model.Name.Trim());
                var proposer = new OrderEmployee();
                proposer = empList.FirstOrDefault();
                var order = _orderService.GetByBId(proposer.batch_Id);
                item.CompanyName = order.CompanyName;
                item.Describe = model.Desc;
                item.openid = model.openid;
                item.ProposerBirthday = proposer.BirBirthday;
                item.ProposerEmail = proposer.Email;
                item.ProposerIdNumber = proposer.IDNumber;
                item.ProposerIdType = proposer.IDType;
                item.ProposerName = proposer.Name;
                item.ProposerPhone = proposer.PhoneNumber;
                item.ProposerSex = proposer.Sex;
                var rEmpList = _orderEmpService.GetByInfo(model.CustomerIdNumber.Trim(), model.Customer.Trim());
                var recipient = new OrderEmployee();
                recipient = rEmpList.FirstOrDefault();
                item.RecipientBirthday = recipient.BirBirthday;
                item.RecipientEmail = recipient.Email;
                item.RecipientIdNumber = recipient.IDNumber;
                item.RecipientIdType = recipient.IDType;
                item.RecipientName = recipient.Name;
                item.RecipientPhone = recipient.PhoneNumber;
                item.RecipientSex = recipient.Sex;
                item.TransformToTPA = false;
                var cid = _claimApiService.insert(item);
                if (cid == 0)
                {
                    return 0;
                }

                string[] invoiceMedia = null;//发票文件mediaid
                string[] caseMedia = null;//病例
                string[] otherMedia = null;//其他资料
                if (!string.IsNullOrEmpty(model.InvoiceList))
                {
                    invoiceMedia = model.InvoiceList.Split(';');
                }
                if (!string.IsNullOrEmpty(model.CaseList))
                {
                    caseMedia = model.CaseList.Split(';');
                }
                if (!string.IsNullOrEmpty(model.OtherList))
                {
                    otherMedia = model.OtherList.Split(';');
                }
                foreach (var a in invoiceMedia)//发票
                {
                    if (!string.IsNullOrEmpty(a))
                    {
                        var fileInfo = new DownLoadWechatFileApi()
                        {
                            MediaId = a,
                            MediaType = "jpg",
                            Url = model.GetMediaUrl + a,
                        };
                        var fid = _archiveService.InsertByWechat(fileInfo);//写入文件
                        if (fid > 0)
                        {
                            var claimFile = new ClaimFileFromWechatItem()
                            {
                                claim_Id = cid,
                                CreateTime = DateTime.Now,
                                FileId = fid,
                                fileType = 1
                            };
                            _clamFileAPiService.insert(claimFile);
                        }
                    }
                }
                foreach (var c in caseMedia)//病例
                {
                    if (!string.IsNullOrEmpty(c))
                    {
                        var fileInfo = new DownLoadWechatFileApi()
                        {
                            MediaId = c,
                            MediaType = "jpg",
                            Url = model.GetMediaUrl + c,
                        };
                        var fid = _archiveService.InsertByWechat(fileInfo);
                        if (fid > 0)
                        {
                            var claimFile = new ClaimFileFromWechatItem()
                            {
                                claim_Id = cid,
                                CreateTime = DateTime.Now,
                                FileId = fid,
                                fileType = 2
                            };
                            _clamFileAPiService.insert(claimFile);
                        }
                    }
                }
                foreach (var o in otherMedia)//其他资料
                {
                    if (!string.IsNullOrEmpty(o))
                    {
                        var fileInfo = new DownLoadWechatFileApi()
                        {
                            MediaId = o,
                            MediaType = "jpg",
                            Url = model.GetMediaUrl + o,
                        };
                        var fid = _archiveService.InsertByWechat(fileInfo);
                        if (fid > 0)
                        {
                            var claimFile = new ClaimFileFromWechatItem()
                            {
                                claim_Id = cid,
                                CreateTime = DateTime.Now,
                                FileId = fid,
                                fileType = 3
                            };
                            _clamFileAPiService.insert(claimFile);
                        }
                    }
                }
                return cid;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimController：PostClaim");
            }
            return 0;
        }
    }
}
