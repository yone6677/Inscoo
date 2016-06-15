using Microsoft.AspNet.Identity.EntityFramework;

namespace Core
{
    public partial class AppRole : IdentityRole
    {
        public string Description { set; get; }
    }
}
