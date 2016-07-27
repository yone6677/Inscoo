using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Api.Claim
{
    public class ClaimDetailsApi : BaseApiModel
    {
        /// <summary>
        /// 赔案号
        /// </summary>
        public string CaseId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ProposerName { get; set; }
        /// <summary>
        /// 申请人性别
        /// </summary>
        public string ProposerSex { get; set; }
        /// <summary>
        /// 申请人生日
        /// </summary>
        public DateTime ProposerBirthday { get; set; }
        /// <summary>
        /// 申请人证件类型
        /// </summary>
        public string ProposerIdType { get; set; }
        /// <summary>
        /// 申请人证件号码
        /// </summary>
        public string ProposerIdNumber { get; set; }
        /// <summary>
        /// 申请人电话
        /// </summary>
        public string ProposerPhone { get; set; }
        /// <summary>
        /// 申请人邮件地址
        /// </summary>
        public string ProposerEmail { get; set; }

        /// <summary>
        /// 出险人
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// 出险人性别
        /// </summary>
        public string RecipientSex { get; set; }
        /// <summary>
        /// 出险人生日
        /// </summary>
        public DateTime RecipientBirthday { get; set; }
        /// <summary>
        /// 出险人证据类型
        /// </summary>
        public string RecipientIdType { get; set; }
        /// <summary>
        /// 出险人证件号码
        /// </summary>
        public string RecipientIdNumber { get; set; }
        /// <summary>
        /// 出险人电话
        /// </summary>
        public string RecipientPhone { get; set; }
        /// <summary>
        /// 出险人邮件地址
        /// </summary>
        public string RecipientEmail { get; set; }
        /// <summary>
        /// 申请描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 发票文件数量
        /// </summary>
        public int InvoiceNum { get; set; }
        /// <summary>
        /// 病例文件数量
        /// </summary>
        public int CaseNum { get; set; }
        /// <summary>
        /// 其他文件
        /// </summary>
        public int OtherNum { get; set; }
    }
}
