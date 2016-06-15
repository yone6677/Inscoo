using Core;
using Core.Identity;
using Microsoft.AspNet.Identity;
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
        //public AppRoleService(ILoggerService loggerService)
        //{
        //    _roleManager = AppRoleManager.Create();
        //    _loggerService = loggerService;
        //}
        public AppRoleService(ILoggerService loggerService, AppRoleManager roleManager)
        {
            _roleManager = roleManager;
            _loggerService = loggerService;
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
                _loggerService.insert(e, LogLevel.Error, "创建角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "删除角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "修改角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "查找角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "查找角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "查找角色出错");
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
                _loggerService.insert(e, LogLevel.Error, "查找角色出错");
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
