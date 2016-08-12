using System;

namespace Domain
{
    public class HealthOrderDetail : BaseEntity
    {
        /// <summary>
        /// 体检订单Id
        /// </summary>
        public int HealthOrderMasterId { set; get; }
        public string Name { set; get; }
        public bool Sex { set; get; }
        public DateTime? Birthday { set; get; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdNumber { set; get; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string Marriage { set; get; }

        /// <summary>
        ///移动电话
        /// </summary>
        public string Phone { set; get; }
        public string Email { set; get; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// 公司名称(Excel上传，可以为空)
        /// </summary>
        public string CompanyName { set; get; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartMent { set; get; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Chair { set; get; }
        /// <summary>
        /// 登录预约系统帐号
        /// </summary>
        public string OrderAccount { set; get; }
        /// <summary>
        /// 登录预约系统密码
        /// </summary>
        public string OrderPassword { set; get; }
        /// <summary>
        /// 登录预约处理时间
        /// </summary>
        public string ProcessDate { set; get; }
        public long Ticks { get; set; }

        public virtual HealthOrderMaster HealthOrderMaster { set; get; }

    }
}
