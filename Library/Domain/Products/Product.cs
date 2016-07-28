using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public class Product : BaseEntity
    {

        public string ProdInsuredName { set; get; }

        public string ProdMemo { set; get; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ItemNo { set; get; }
        /// <summary>
        /// 产品类别
        /// </summary>
        public string ProdType { set; get; }
        /// <summary>
        /// 保障方案代码
        /// </summary>
        public string SafeguardCode { set; get; }
        /// <summary>
        /// 保障方案名称
        /// </summary>
        public string SafeguardName { set; get; }
        /// <summary>
        /// 被保险人类型
        /// </summary>
        public string InsuredWho { set; get; }
        /// <summary>
        /// 保额
        /// </summary>
        public string CoverageSum { set; get; }
        /// <summary>
        /// 赔付比例
        /// </summary>
        public string PayoutRatio { set; get; }
        /// <summary>
        /// 3-4人
        /// </summary>
        public string HeadCount3 { set; get; }
        /// <summary>
        /// 5-10人
        /// </summary>
        public string HeadCount5 { set; get; }
        /// <summary>
        /// 11-30人
        /// </summary>
        public string HeadCount11 { set; get; }
        /// <summary>
        /// 31-50人
        /// </summary>
        public string HeadCount31 { set; get; }
        /// <summary>
        /// 50-99人
        /// </summary>
        public string HeadCount51 { set; get; }
        /// <summary>
        /// 100人及以上
        /// </summary>
        public string HeadCount100 { set; get; }
        /// <summary>
        /// 保险公司
        /// </summary>
        public string InsuredCom { set; get; }
        /// <summary>
        /// 理赔代码
        /// </summary>
        public string ClaimCode { get; set; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        public float CommissionRate { get; set; }

        public string changer { get; set; }

        public string ProvisionPath { get; set; }
        /// <summary>
        /// 产品期限
        /// </summary>
        public string ProdTimeLimit { get; set; }
        /// <summary>
        /// 是否可以退保
        /// </summary>
        public bool ProdWithdraw { get; set; }
        /// <summary>
        /// 免赔额
        /// </summary>
        public string ProdAbatement { get; set; }
        /// <summary>
        /// 报价方式
        /// </summary>
        public string ProdQuoteType { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        public string ProdPayType { get; set; }
        /// <summary>
        /// 投保单生成方式
        /// </summary>
        public string ProdCreateType { get; set; }
    }
}
