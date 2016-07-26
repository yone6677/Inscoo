using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Api.Claim
{
    public class ClaimModel : BaseApiModel
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 申请人证件号
        /// </summary>
        public string IdNumer { get; set; }
        /// <summary>
        /// 出险人
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 出险人证件号
        /// </summary>
        public string CustomerIdNumber { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 发票
        /// </summary>
        public string InvoiceList { get; set; }
        /// <summary>
        /// 病例
        /// </summary>
        public string CaseList { get; set; }
        /// <summary>
        /// 证件及其他
        /// </summary>
        public string OtherList { get; set; }
        /// <summary>
        /// 微信获取文件url
        /// </summary>
        public string GetMediaUrl { get; set; }
        public string openid { get; set; }
    }
}
