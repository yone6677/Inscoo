
namespace Domain.Claim
{
    public class ClaimFileFromWechatItem : BaseEntity
    {
        public int FileId { get; set; }
        public ClaimFromWechatItem claim { get; set; }
        public int claim_Id { get; set; }
        /// <summary>
        /// 文件类型/1 发票,2.病例,3.其他
        /// </summary>
        public int fileType { get; set; }
    }
}
