using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class HealthCheckProduct : BaseEntity
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        public string ProductType { set; get; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { set; get; }
        public string ProductName { set; get; }
        public string ProductMemo { set; get; }
        /// <summary>
        /// 体检公司代码
        /// </summary>
        public string CompanyCode { set; get; }
        public string CompanyName { set; get; }
        /// <summary>
        /// 体检费成本价
        /// </summary>
        public decimal PublicPrice { set; get; }
        /// <summary>
        /// 体检费BD对外售价
        /// </summary>
        public decimal BdPrice { set; get; }
        /// <summary>
        /// 体检费Channel对外售价
        /// </summary>
        public decimal ChannelPrice { set; get; }
        /// <summary>
        /// 体检费Hr对外售价
        /// </summary>
        public decimal HrPrice { set; get; }
        /// <summary>
        /// 体检费Other对外售价
        /// </summary>
        public decimal OtherPrice { set; get; }
        /// <summary>
        /// 佣金计算方法
        /// </summary>
        public string CommissionMethod { set; get; }
        /// <summary>
        /// 产品图片路径
        /// </summary>
        public string CheckProductPic { set; get; }

    }
}
