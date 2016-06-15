using Core.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(IRoleStore<AppRole,string> store) 
            : base(store)
        {
        }
        //public AppRoleManager()
        //    : base(new RoleStore<AppRole>(new AppDbContext()))
        //{
        //}
        //public static AppRoleManager Create()
        //{
        //    var manager = new AppRoleManager();
        //    return manager;
        //}
    }
}
