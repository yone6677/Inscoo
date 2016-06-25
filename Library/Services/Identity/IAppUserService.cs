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
using System.Web.Mvc;

namespace Services.Identity
{
    public interface IAppUserService
    {
        Task<IdentityResult> AddToRoleAsync(string uid, string roleid);
        bool ChangePassword(string id, string oldPassword, string password);
        Task<IdentityResult> CreateAsync(AppUser user, string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string appCookie);
        ClaimsIdentity CreateIdentity(AppUser user, string appCookie);
        Task<IdentityResult> DeleteAsync(AppUser user);


        AppUser FindByEmail(string email);
        AppUser FindByName(string name);
        AppUser FindById(string id);
        AppUser Find(string userName, string password);
        AppUser Find(UserLoginInfo info);


        /// <summary>
        /// 获得当前用户
        /// </summary>
        /// <returns></returns>
        AppUser GetCurrentUser();
        UserModel Get_UserModel_ById(string id);
        string GetRoleByUserId(string uId);
        List<string> GetRolesByUserId(string uId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="valueField">该参数值只能是Name或Id</param>
        /// <returns></returns>
        List<SelectListItem> GetRolesManagerPermissionByUserId(string uId, string valueField = "Name");
        IPagedList<UserModel> GetUserList(int pageIndex = 1, int pageSize = 15, string userName = null, string email = null, string role = "",string roleId = "");
        List<UserRoleModel> GetUserRoles();


        void SignIn(AppUser user, bool isPersistent);
        void SignOut();

        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);

    }
}
