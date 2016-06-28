
namespace Domain
{
    public class BusinessLicense : BaseFile
    {
        public int CompanyId { set; get; }
        public virtual Company Company { set; get; }
    }
}
