
namespace Models.Order
{
    public class UploadInfoModel : BaseViewModel
    {
        /// <summary>
        /// 人员信息PDF
        /// </summary>
        public string EmpInfoFilePDFUrl { get; set; }
        /// <summary>
        /// 是否上传人员信息PDF盖章文件
        /// </summary>
        public bool HasEmpInfoFilePDFSeal { get; set; }
        /// <summary>
        /// 人员信息PDF盖章文件
        /// </summary>
        public string EmpInfoFilePDFSealUrl { get; set; }
        /// <summary>
        /// 投保单模板
        /// </summary>
        public string InsurancePolicyTemp { get; set; }
        /// <summary>
        /// 是否上传投保单盖章文件
        /// </summary>
        public bool HasInsurancePolicy { get; set; }
        /// <summary>
        /// 投保单盖章文件
        /// </summary>
        public string InsurancePolicySealUrl { get; set; }
        /// <summary>
        /// 营业执照样本
        /// </summary>
        public string BusinessLicenseTemp { get; set; }
        /// <summary>
        /// 是否上传营业执照盖章文件
        /// </summary>
        public bool HasBusinessLicense { get; set; }
        /// <summary>
        /// 营业执照盖章文件
        /// </summary>
        public string BusinessLicenseSealUrl { get; set; }
    }
}
