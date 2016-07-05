using Core;
using Core.Pager;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Models.Role;
using Models.User;
using Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public interface IAppUserService
    {
        Task<IdentityResult> AddToRoleAsync(string uid, string roleName);

        bool ChangePassword(string id, string oldPassword, string password);
        Task<IdentityResult> CreateAsync(AppUser user, string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string appCookie);
        ClaimsIdentity CreateIdentity(AppUser user, string appCookie);
        Task<IdentityResult> DeleteAsync(AppUser user);
        bool DeleteBeforeRoleAndNew(string uid, string roleName);

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
        RegisterModel Get_RegisterModel_ById(string id);
        string GetRoleByUserId(string uId);
        List<string> GetRolesByUserId(string uId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="valueField">该参数值只能是Name或Id</param>
        /// <returns></returns>
        SelectList GetRolesManagerPermissionByUserId(string uId, string valueField, string selectedValue = "");
        IPagedList<UserModel> GetUserList(int pageIndex = 1, int pageSize = 15, string userName = null, string email = null, string role = "", string roleId = "", string createUserId = "-1");
        List<UserRoleModel> GetUserRoles();

        /// <summary>
        /// 判断是否用户名已使用。用户名和邮箱都不能存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsUserExist(string key);
        void SignIn(AppUser user, bool isPersistent);
        void SignOut();

        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);

    }
}
