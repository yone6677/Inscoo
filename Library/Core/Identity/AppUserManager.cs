using Core.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace Core.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }
        //public AppUserManager()
        //    : base(new UserStore<AppUser>(new AppDbContext()))
        //{
        //}
        //public static AppUserManager Create()
        //{
        //    return new AppUserManager();
        //}
    }
}
