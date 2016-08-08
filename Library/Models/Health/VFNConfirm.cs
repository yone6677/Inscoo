using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    /// <summary>
    /// 财务确认收款
    /// </summary>
    public class VFNConfirm
    {
        public int MasterId { set; get; }
        public string DateTicks { set; get; }
        [Display(Name = "付款日期")]
        [Required]
        public DateTime FinancePayDate { set; get; }

        [Display(Name = "收款金额")]
        [Required]
        public decimal FinanceAmount { set; get; }

        [Display(Name = "银行转帐流水号")]
        [Required]
        public string FinanceBankSerialNumber { set; get; }

        [Display(Name = "备注")]
        public string FinanceMemo { set; get; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}