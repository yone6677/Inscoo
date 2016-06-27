
namespace Domain
{
    public class BusinessLicense : BaseFile
    {
        public string UserId { set; get; }

        public virtual AppUser AppUser { set; get; }
    }
}
