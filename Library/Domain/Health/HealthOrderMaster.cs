using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class HealthOrderMaster : BaseEntity
    {
        public int HealthCheckProductId { set; get; }
        /// <summary>
        /// 体检费成本价（人/次）
        /// </summary>
        public decimal PublicPrice { set; get; }
        /// <summary>
        /// 体检费对外售价
        /// </summary>
        public decimal SellPrice { set; get; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        public int CommissionRatio { set; get; }
        /// <summary>
        /// 佣金计算方法
        /// </summary>
        public string CommissionMethod { set; get; }

        /// <summary>
        /// 体检开始日期
        /// </summary>
        public DateTime? CheckBeginTime { set; get; }
        public DateTime? CheckEndTime { set; get; }
        public int? CompanyId { set; get; }

        public string BaokuOrderCode { set; get; }

        public int Status { set; get; }

        public string PersonExcelPath { set; get; }

        public string BaokuConfirmer { set; get; }
        /// <summary>
        /// 保酷确定日期
        /// </summary>
        public DateTime? BaokuConfirmDate { set; get; }

        /// <summary>
        /// 财务确认人
        /// </summary>
        public string FinanceConfirmer { set; get; }
        /// <summary>
        /// 财务确认日期
        /// </summary>
        public DateTime? FinanceConfirmDate { set; get; }
        /// <summary>
        /// 财务输入付款日期
        /// </summary>
        public DateTime? FinancePayDate { set; get; }
        /// <summary>
        /// 财务输入收款金额
        /// </summary>
        public decimal FinanceAmount { set; get; }
        /// <summary>
        /// 财务输入银行转账流水号
        /// </summary>
        public string FinanceBankSerialNumber { set; get; }
        /// <summary>
        /// 财务输入其他备注
        /// </summary>
        public string FinanceMemo { set; get; }

        /// <summary>
        /// 体检公司确认人
        /// </summary>
        public string CheckComConfirmer { set; get; }
        /// <summary>
        /// 体检公司输入订单号
        /// </summary>
        public string CheckComOrderCode { set; get; }
        /// <summary>
        /// 体检公司确认日期
        /// </summary>
        public DateTime? CheckComConfirmDate { set; get; }
        /// <summary>
        /// 体检公司输入快递公司
        /// </summary>
        public string ExpressCom { set; get; }
        /// <summary>
        /// 体检公司输入快递单号
        /// </summary>
        public string ExpressCode { set; get; }

        /// <summary>
        /// 体检公司输入其他备注
        /// </summary>
        public string CheckComMemo { set; get; }

        public virtual HealthCheckProduct HealthCheckProduct { set; get; }
        public virtual Company Company { set; get; }
    }
}
