
namespace Domain.Claim
{
    public class ClaimFileFromWechatItem : BaseEntity
    {
        public int FileId { get; set; }
        public ClaimFromWechatItem claim { get; set; }
        public int claim_Id { get; set; }
    }
}
