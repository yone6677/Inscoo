using System.ComponentModel;

namespace Models.Infrastructure
{
    /// <summary>
    /// 保全类型
    /// </summary>
    public enum PMType
    {
        [Description("新契约")]
        PM00,
        [Description("被保险人生日变更")]
        PM01,
        [Description("被保险人地址变更")]
        PM02,
        [Description("被保险人联系方式变更")]
        PM03,
        [Description("被保险人国籍变更")]
        PM04,
        [Description("被保险人证件号码变更")]
        PM05,
        [Description("被保险人姓名变更")]
        PM06,
        [Description("被保险人性别变更")]
        PM07,
        [Description("被保险人银行账号变更")]
        PM08,
        [Description("被保险人职业类别变更")]
        PM09,
        [Description("被保险人受益人变更")]
        PM10,
        [Description("被保险人特别约定变更")]
        PM11,
        [Description("被保险人生效日期变更")]
        PM12,
        [Description("被保险人终止日期变更")]
        PM13,
        [Description("被保险人计划变更")]
        PM14,
        [Description("增加被保险人")]
        PM15,
        [Description("终止被保险人")]
        PM16,
        [Description("员工信息变更")]
        PM17,
        [Description("被保险人复效")]
        PM18
    }
}
