using System;
using System.Collections.Generic;

namespace Domain.Claim
{
    public class ClaimFromWechatItem : BaseEntity
    {
        /// <summary>
        /// 赔案号
        /// </summary>
        public string CaseId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ProposerName { get; set; }
        /// <summary>
        /// 申请人性别
        /// </summary>
        public string ProposerSex { get; set; }
        /// <summary>
        /// 申请人生日
        /// </summary>
        public DateTime ProposerBirthday { get; set; }
        /// <summary>
        /// 申请人证件类型
        /// </summary>
        public string ProposerIdType { get; set; }
        /// <summary>
        /// 申请人证件号码
        /// </summary>
        public string ProposerIdNumber { get; set; }
        /// <summary>
        /// 申请人电话
        /// </summary>
        public string ProposerPhone { get; set; }
        /// <summary>
        /// 申请人邮件地址
        /// </summary>
        public string ProposerEmail { get; set; }

        /// <summary>
        /// 出险人
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// 出险人性别
        /// </summary>
        public string RecipientSex { get; set; }
        /// <summary>
        /// 出险人生日
        /// </summary>
        public DateTime RecipientBirthday { get; set; }
        /// <summary>
        /// 出险人证据类型
        /// </summary>
        public string RecipientIdType { get; set; }
        /// <summary>
        /// 出险人证件号码
        /// </summary>
        public string RecipientIdNumber { get; set; }
        /// <summary>
        /// 出险人电话
        /// </summary>
        public string RecipientPhone { get; set; }
        /// <summary>
        /// 出险人邮件地址
        /// </summary>
        public string RecipientEmail { get; set; }
        /// <summary>
        /// 申请描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 合并后的图片
        /// </summary>
        public string FullImage { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 是否抛砖至TPA
        /// </summary>
        public bool TransformToTPA { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public ICollection<ClaimFileFromWechatItem> ImageList { get; set; }
    }
}
