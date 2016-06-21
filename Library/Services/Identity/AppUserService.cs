using Core;
using Core.Data;
using Core.Identity;
using Core.Pager;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models.User;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Services.Identity
{
    public class AppUserService : IAppUserService
    {
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _appRoleManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public AppUserService(ILoggerService loggerService, AppUserManager userManager, AppRoleManager appRoleManager,
            IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _appRoleManager = appRoleManager;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }
        public Task<IdentityResult> CreateAsync(AppUser user, string name, string password)
        {
            try
            {
                var result = _userManager.CreateAsync(user, password);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppUserService:CreateAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string appCookie)
        {
            try
            {
                var result = _userManager.CreateIdentityAsync(user, appCookie);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:CreateIdentityAsync", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public ClaimsIdentity CreateIdentity(AppUser user, string appCookie)
        {
            try
            {
                var result = _userManager.CreateIdentity(user, appCookie);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:CreateIdentity", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public bool ChangePassword(string id, string oldPassword, string password)
        {
            try
            {
                var result = _userManager.ChangePassword(id, oldPassword, password);
                if (result.Succeeded) return true;
                else
                {
                    throw new Exception(result.Errors.First());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public Task<IdentityResult> DeleteAsync(AppUser user)
        {
            try
            {
                var result = _userManager.DeleteAsync(user);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:DeleteAsync", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public Task<IdentityResult> UpdateAsync(AppUser user)
        {
            try
            {
                user.Changer = _authenticationManager.User.Identity.Name;
                user.ModifyTime = DateTime.Now;
                var result = _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:UpdateAsync", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public Task<IdentityResult> UpdateSecurityStampAsync(AppUser user)
        {
            try
            {
                user.Changer = _authenticationManager.User.Identity.Name;
                user.ModifyTime = DateTime.Now;
                var result = _userManager.UpdateSecurityStampAsync(user.Id);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:UpdateSecurityStampAsync", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public AppUser FindByEmail(string email)
        {

            try
            {
                var result = _userManager.FindByEmail(email);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindByEmail", _authenticationManager.User.Identity.Name);
                return null;
            }

        }
        public AppUser FindByName(string name)
        {
            try
            {
                var result = _userManager.FindByName(name);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindByName", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public AppUser FindById(string id)
        {

            try
            {
                var result = _userManager.FindById(id);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindById", _authenticationManager.User.Identity.Name);
                return null;
            }

        }
        public AppUser Find(string userName, string password)
        {
            try
            {
                var result = _userManager.Find(userName, password);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:Find", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public AppUser Find(UserLoginInfo info)
        {
            try
            {
                var result = _userManager.Find(info);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:Find", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public Task<IdentityResult> AddToRoleAsync(string uid, string roleid)
        {
            try
            {
                var user = _userManager.AddToRoleAsync(uid, roleid);
                return user;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:AddToRoleAsync", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
        public void SignIn(AppUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        public void SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        public IPagedList<UserModel> GetUserList(int pageIndex, int pageSize, string userName, string email)
        {
            try
            {
                var model = new List<UserModel>();
                var user = _userManager.Users.ToList();
                if (!string.IsNullOrEmpty(userName))
                {
                    user = user.Where(s => s.UserName == userName).ToList();
                }
                if (!string.IsNullOrEmpty(email))
                {
                    user = user.Where(s => s.Email == email).ToList();
                }
                if (user.Count > 0)
                {
                    model = user.Select(u => new UserModel
                    {
                        CompanyName = u.CompanyName,
                        Id = u.Id,
                        Name = u.UserName,
                        Email = u.Email,
                        Phone = u.PhoneNumber,
                        LinkMan = u.LinkMan,
                        FanBao = u.FanBao,
                        TiYong = u.TiYong,
                        RoleIds = u.Roles.Any() ? u.Roles.Select(r => r.RoleId).ToList() : null,
                        RoleName = u.Roles.Any() ? _appRoleManager.FindById(u.Roles.First().RoleId).Name : "",
                        CreateTime = u.CreateTime,
                        CreaterId = u.CreaterId
                    }).ToList();
                }

                return new PagedList<UserModel>(model, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:GetUserList", _authenticationManager.User.Identity.Name);
                return null;
            }
        }
    }
}
