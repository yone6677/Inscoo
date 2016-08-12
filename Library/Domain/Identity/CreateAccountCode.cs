using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Domain

{
    public partial class CreateAccountCode : BaseEntity
    {
        public CreateAccountCode()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
            IsUsed = false;
            EncryFanBao = false;
            EncryTiYong = false;
            EncryRebate = 0;
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        public string AccountEncryCode { set; get; }
        /// <summary>
        /// 用户是否已使用
        /// </summary>
        public bool IsUsed { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string EncryCreateID { set; get; }
        /// <summary>
        /// 有效开始日期
        /// </summary>
        public DateTime? EncryBeginDate { set; get; }
        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime? EncryEndDate { set; get; }
        /// <summary>
        /// 角色
        /// </summary>
        public string EncryRoleName { set; get; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EncryCompanyName { set; get; }
        /// <summary>
        /// 佣金计算方法
        /// </summary>
        public string EncryCommissionMethod { set; get; }
        /// <summary>
        /// 自选产品权限(保险公司)
        /// </summary>
        public string EncryInsurance { get; set; }
        /// <summary>
        /// 专属产品权限
        /// </summary>
        public string EncrySeries { get; set; }
        /// <summary>
        /// 理赔比率
        /// </summary>
        public bool EncryFanBao { get; set; }
        /// <summary>
        /// 利润加成
        /// </summary>
        public bool EncryTiYong { get; set; }
        /// <summary>
        /// 返点分配
        /// </summary>
        public int EncryRebate { get; set; }
        /// <summary>
        /// 其他备注
        /// </summary>
        public string EncryMemo { set; get; }
    }
}
