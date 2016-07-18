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


        public DateTime CheckBeginTime { set; get; }
        public DateTime CheckEndTime { set; get; }
        public int CompanyId { set; get; }

        public string BaokuOrderCode { set; get; }

        public int Status { set; get; }

        public string PersonExcelPath { set; get; }

        public string BaokuConfirmer { set; get; }

        public DateTime BaokuConfirmDate { set; get; }


        public string FinanceConfirmer { set; get; }

        public DateTime FinanceConfirmDate { set; get; }

        public DateTime FinancePayDate { set; get; }
        public decimal FinanceAmount { set; get; }

        public string FinanceBankSerialNumber { set; get; }
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
        public string CheckComConfirmDate { set; get; }
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
