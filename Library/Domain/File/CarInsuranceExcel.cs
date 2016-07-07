
namespace Domain
{
    public class CarInsuranceExcel : BaseFile
    {
        public string AppUserId { set; get; }
        public virtual AppUser AppUser { set; get; }
    }
}
