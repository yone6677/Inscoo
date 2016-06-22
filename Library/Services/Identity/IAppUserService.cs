using Core;
using Core.Pager;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Models.Role;
using Models.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Identity
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUser user, string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string appCookie);
        ClaimsIdentity CreateIdentity(AppUser user, string appCookie);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);
        AppUser FindByEmail(string email);
        AppUser FindByName(string name);
        AppUser FindById(string id);
        AppUser Find(string userName, string password);
        AppUser Find(UserLoginInfo info);
        Task<IdentityResult> AddToRoleAsync(string uid, string roleid);
        void SignIn(AppUser user, bool isPersistent);
        void SignOut();
        IPagedList<UserModel> GetUserList(int pageIndex = 1, int pageSize = 15, string userName = null, string email = null);
        List<UserRoleModel> GetUserRoles();
        /// <summary>
        /// 获得当前用户
        /// </summary>
        /// <returns></returns>
        AppUser GetCurrentUser();
    }
}
