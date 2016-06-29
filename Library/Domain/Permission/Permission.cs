
namespace Domain
{
    public partial class Permission : BaseEntity
    {
        public string roleId { get; set; }
        public int NavigationId { set; get; }
        public virtual Navigation Navigation { get; set; }
    }
}
