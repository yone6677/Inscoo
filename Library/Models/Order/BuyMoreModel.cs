using System.ComponentModel;

namespace Models.Order
{
    public class BuyMoreModel : BaseViewModel
    {
        /// <summary>
        /// 批次ID
        /// </summary>
        public int BId { get; set; }
        /// <summary>
        /// 是否允许变更资料
        /// </summary>
        public bool Disable { get; set; }
        [DisplayName("人员信息模板")]
        public string EmpInfoFileUrl { get; set; }
        /// <summary>
        /// 已上传的excel
        /// </summary>
        [DisplayName("人员信息Excel")]
        public string EmpInfoExcel { get; set; }
        /// <summary>
        /// 是否已上传Excel
        /// </summary>
        public bool HasEmpInfoExcel { get; set; }
        /// <summary>
        /// 产生的PDF
        /// </summary>
        [DisplayName("加减保员工名单(加盖公章)")]
        public string EmpInfoFilePDF { get; set; }
        /// <summary>
        /// 是否产生PDF
        /// </summary>
        public bool HasEmpInfoFilePDF { get; set; }
        /// <summary>
        /// 人员信息PDF盖章文件
        /// </summary>
        public string EmpInfoFilePDFSealUrl { get; set; }
        /// <summary>
        /// 是否上传人员信息PDF盖章文件
        /// </summary>
        public bool HasEmpInfoFilePDFSeal { get; set; }
        public bool HasPaymentNotice { get; set; }
        [DisplayName("付款通知书")]
        public string PaymentNotice { get; set; }
        public string Result { get; set; }
    }
}
