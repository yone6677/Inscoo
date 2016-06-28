using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain
{
    public partial class AppRole : IdentityRole
    {
        public string Description { set; get; }
    }
}
