
using System;

namespace Domain
{
    public class ClaimManagementDetail : BaseEntity
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string ClaimBatch { get; set; }
        /// <summary>
        /// 团体号
        /// </summary>
        public string InsuranceGroup { get; set; }
        /// <summary>
        /// 团体名称
        /// </summary>
        public string InsuranceGroupName { get; set; }
        /// <summary>
        /// 子团体号
        /// </summary>
        public string InsuranceSubGroup { get; set; }
        /// <summary>
        /// 子团体客户号
        /// </summary>
        public string InsuranceSubCustomerCode { get; set; }
        /// <summary>
        /// 子团体名称
        /// </summary>
        public string InsuranceSubCustomerName { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        public string InsuranceNo { get; set; }
        /// <summary>
        /// 被保险人保险生效日期
        /// </summary>
        public DateTime InsuranceEffectiveDate { get; set; }
        /// <summary>
        /// 被保险人保险终止日期
        /// </summary>
        public DateTime InsuranceExpiryDate { get; set; }
        /// <summary>
        /// 就诊日期距离被保险人生效日期天数
        /// </summary>
        public string SpaceDay { get; set; }
        /// <summary>
        /// 所属主被保险人姓名
        /// </summary>
        public string BeloneInsuranceName { get; set; }
        /// <summary>
        /// 被保险人姓名
        /// </summary>
        public string InsuranceName { get; set; }
        /// <summary>
        /// 与主保险人关系
        /// </summary>
        public string InsuranceRel { get; set; }
        /// <summary>
        /// 被保险人性别
        /// </summary>
        public string InsuranceSex { get; set; }
        /// <summary>
        /// 被保险人证件号码
        /// </summary>
        public string InsuranceManID { get; set; }
        /// <summary>
        /// 被保险人出生日期
        /// </summary>
        public string InsuranceBirthDay { get; set; }
        /// <summary>
        /// 被保险人证年龄
        /// </summary>
        public string InsuranceAge { get; set; }
        /// <summary>
        /// 被保险人社保标识
        /// </summary>
        public string InsuranceSBflag { get; set; }
        /// <summary>
        /// 被保险人社保地
        /// </summary>
        public string InsuranceSBcity { get; set; }
        /// <summary>
        /// 赔案号
        /// </summary>
        public string ClaimCaseID { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ClaimApplyDate { get; set; }
        /// <summary>
        /// 受理日期
        /// </summary>
        public DateTime ClaimOperDate { get; set; }
        /// <summary>
        /// 赔付日期
        /// </summary>
        public DateTime ClaimPayDate { get; set; }
        /// <summary>
        /// 事故日期
        /// </summary>
        public DateTime ClaimAccdtDate { get; set; }

        /// <summary>
        /// 就诊起始日期
        /// </summary>
        public DateTime ClaimDoctDate { get; set; }
        /// <summary>
        /// 就诊结束日期
        /// </summary>
        public DateTime ClaimDoctFDate { get; set; }
        /// <summary>
        /// 赔付类型
        /// </summary>
        public string ClaimType { get; set; }
        /// <summary>
        /// 客户给付险种名称
        /// </summary>
        public string ClaimInsName { get; set; }
        /// <summary>
        /// 产品号
        /// </summary>
        public string InsProdID { get; set; }
        /// <summary>
        /// 医院代码
        /// </summary>
        public string HospitalCode { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 医院地址
        /// </summary>
        public string HospitalCity { get; set; }
        /// <summary>
        /// 医院类型
        /// </summary>
        public string HospitalType { get; set; }
        /// <summary>
        /// 医院等级
        /// </summary>
        public string HospitalLevel { get; set; }
        /// <summary>
        /// 医院类别
        /// </summary>
        public string HospitalKind { get; set; }
        /// <summary>
        /// 特需医院
        /// </summary>
        public string HospitalSpecial { get; set; }
        /// <summary>
        /// 社保定点医院
        /// </summary>
        public string HospitalYBDD { get; set; }
        /// <summary>
        /// 疾病代码
        /// </summary>
        public string DiseaseCode { get; set; }
        /// <summary>
        /// 疾病描述
        /// </summary>
        public string DiseaseDisc { get; set; }
        /// <summary>
        /// 次数天数
        /// </summary>
        public string TimesDay { get; set; }
        /// <summary>
        /// 理赔结论
        /// </summary>
        public string ClaimResult { get; set; }
        /// <summary>
        /// 费用总额
        /// </summary>
        public string ExpTotal { get; set; }
        /// <summary>
        /// 药品总费用
        /// </summary>
        public string ExpTotalDrog { get; set; }
        /// <summary>
        /// 账户支付
        /// </summary>
        public string PayFromAccount { get; set; }
        /// <summary>
        /// 统筹支付
        /// </summary>
        public string PayForGov { get; set; }
        /// <summary>
        /// 分类支付
        /// </summary>
        public string PayKind { get; set; }
        /// <summary>
        /// 自费
        /// </summary>
        public string PaySelf { get; set; }
        /// <summary>
        /// 第三方支付
        /// </summary>
        public string PayThiryPart { get; set; }
        /// <summary>
        /// 审核金额
        /// </summary>
        public string ApplyAmt { get; set; }
        /// <summary>
        /// 免赔付
        /// </summary>
        public string Deductible { get; set; }
        /// <summary>
        /// 次免赔付
        /// </summary>
        public string DeductibleTime { get; set; }
        /// <summary>
        /// 共保金额
        /// </summary>
        public string ClaimSum { get; set; }
        /// <summary>
        /// 赔付金额
        /// </summary>
        public string ClaimAmt { get; set; }
        /// <summary>
        /// 本次扣款金额
        /// </summary>
        public string DebitAmt { get; set; }
        /// <summary>
        /// 扣除明细
        /// </summary>
        public string DebitDiscript { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
