using System;
using System.Collections.Generic;

namespace Domain.Orders
{
    public class OrderBatch : BaseEntity
    {
        public int order_Id { get; set; }
        /// <summary>
        /// 投保批次号
        /// </summary>
        public string BNum { get; set; }
        /// <summary>
        /// 投保批次状态 0.待审核/1.Inscoo已同意/2.Inscoo已拒绝/3.财务已同意/4.财务已拒绝/5.保险公司已同意/7.保险公司已拒绝/8.已确认提交加减保
        /// </summary>
        public int BState { get; set; }
        /// <summary>
        /// 人员资料文件
        /// </summary>
        public int EmpInfoFile { get; set; }
        /// <summary>
        /// 人员资料PDF文件
        /// </summary>
        public int EmpInfoFilePDF { get; set; }
        /// <summary>
        /// 人员资料盖章文件
        /// </summary>
        public int EmpInfoFileSeal { get; set; }
        /// <summary>
        /// 投保单盖章文件
        /// </summary>
        public int PolicySeal { get; set; }
        /// <summary>
        /// 投保单待盖章文件
        /// </summary>
        public int PolicyPDF { get; set; }
        /// <summary>
        /// 付款通知书
        /// </summary>
        public int PaymentNoticePDF { get; set; }
        /// <summary>
        /// 投保操作人
        /// </summary>
        public string PolicyHolder { get; set; }
        /// <summary>
        /// 投保操作日期
        /// </summary>
        public DateTime PolicyHolderDate { get; set; }
        /// <summary>
        /// 保酷确认人
        /// </summary>
        public string InscooConfirm { get; set; }
        /// <summary>
        /// 保酷确认日期
        /// </summary>
        public DateTime InscooConfirmDate { get; set; }
        /// <summary>
        /// 财务确认人
        /// </summary>
        public string Finance { get; set; }
        /// <summary>
        /// 财务确认日期
        /// </summary>
        public DateTime FinanceDate { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal AmountCollected { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime CollectionDate { get; set; }
        /// <summary>
        /// 转账凭证
        /// </summary>
        public string TransferVoucher { get; set; }
        /// <summary>
        /// 财务备注
        /// </summary>
        public string FinanceMemo { get; set; }
        /// <summary>
        /// 保险公司确认人
        /// </summary>
        public string Insurer { get; set; }
        /// <summary>
        /// 保险公司确认日期
        /// </summary>
        public DateTime InsurerConfirmDate { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string CourierNumber { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string Express { get; set; }
        /// <summary>
        /// 保险公司备注
        /// </summary>
        public string InsurerMemo { get; set; }
        /// <summary>
        /// 人员详细
        /// </summary>
        public virtual ICollection<OrderEmployee> orderEmp { get; set; }
        public virtual Order order { get; set; }
    }
}
