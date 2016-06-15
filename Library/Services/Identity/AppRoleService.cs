using Core;
using Core.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Identity
{
    public class AppRoleService : IAppRoleService
    {
        private readonly AppRoleManager _roleManager;
        private readonly ILoggerService _loggerService;
        private readonly IAuthenticationManager _authenticationManager;
        public AppRoleService(ILoggerService loggerService, AppRoleManager roleManager, IAuthenticationManager authenticationManager)
        {
            _roleManager = roleManager;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }
        public Task<IdentityResult> CreateAsync(AppRole role)
        {
            try
            {
                var result = _roleManager.CreateAsync(role);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:CreateAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<IdentityResult> DeleteAsync(AppRole role)
        {
            try
            {
                var result = _roleManager.DeleteAsync(role);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:DeleteAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<IdentityResult> UpdateAsync(AppRole role)
        {
            try
            {
                var result = _roleManager.UpdateAsync(role);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:UpdateAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<AppRole> FindByIdAsync(string roleid)
        {
            try
            {
                var result = _roleManager.FindByIdAsync(roleid);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:FindByIdAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<AppRole> FindByNameAsync(string name)
        {
            try
            {
                var result = _roleManager.FindByNameAsync(name);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:FindByNameAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public Task<bool> RoleExistsAsync(string name)
        {
            try
            {
                var result = _roleManager.RoleExistsAsync(name);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:RoleExistsAsync", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public IQueryable<AppRole> Roles()
        {
            try
            {
                var result = _roleManager.Roles;
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppRoleService:Roles", _authenticationManager.User.Identity.Name);
                throw e;
            }
        }
        public List<SelectListItem> GetSelectList()
        {
            var table = Roles();
            if (table.Any())
            {
                var select = new List<SelectListItem>();
                foreach (var s in table)
                {
                    var item = new SelectListItem();
                    item.Text = s.Description;
                    item.Value = s.Name;
                    select.Add(item);
                }
                return select;
            }
            return null;
        }
    }
}
