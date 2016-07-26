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
        [ResponseType(typeof(ClaimFromWechatItem))]
        public IHttpActionResult GetById(int id)
        {
            var item = _claimApiService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
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
                foreach (var c in invoiceMedia)//病例
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
                foreach (var o in invoiceMedia)//其他资料
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
                                fileType = 1
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
