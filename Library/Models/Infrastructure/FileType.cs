using System.ComponentModel;

namespace Models.Infrastructure
{
    public enum FileType
    {
        [Description("投保人员信息Excel")]
        EmployeeInfo,
        [Description("投保人员信息PDF")]
        EmployeeInfoSeal,
        [Description("投保人员信息(加盖公章)PDF")]
        EmployeeInfoPDF,
        [Description("投保单(加盖公章)")]
        PolicySeal,
        [Description("营业执照(加盖公章)")]
        BusinessLicenseSeal,
        [Description("付款通知书")]
        PaymentNotice
    }
}
