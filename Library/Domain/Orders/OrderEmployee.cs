using System;

namespace Domain.Orders
{
   public class OrderEmployee:BaseEntity
    {        
        /// <summary>
        /// 投保批次号
        /// </summary>
        public string BNum { get; set; }
        /// <summary>
        /// 保全代码
        /// </summary>
        public string PMCode { get; set; }
        /// <summary>
        /// 保全名称
        /// </summary>
        public string PMName { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IDType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime BirBirthday { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public decimal Premium { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BankName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 是否有社保
        /// </summary>
        public string HasSocialSecurity { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        public virtual Order order { get; set; }
        public int order_Id { get; set; }
    }
}
