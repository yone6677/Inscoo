using Core.Data;
using Domain.Permission;
using Microsoft.Owin.Security;
using Models.Navigation;
using Services.Identity;
using Services.Infrastructure;
using Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<PermissionItem> _permissionRepository;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly INavigationService _navService;

        public PermissionService(ILoggerService loggerService, IRepository<PermissionItem> permissionRepository, IAppUserService appUserService, IAppRoleService appRoleService,
            INavigationService navService)
        {
            _loggerService = loggerService;
            _permissionRepository = permissionRepository;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
            _navService = navService;
        }
        public bool Insert(PermissionItem item)
        {
            try
            {
                _permissionRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
                return false;
            }

        }
        public bool Update(PermissionItem item)
        {
            try
            {
                _permissionRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Update");
                return false;
            }
        }
        public bool Delete(PermissionItem item)
        {
            try
            {
                _permissionRepository.Delete(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Delete");
                return false;
            }

        }
        public bool DeleteById(int id)
        {
            try
            {
                _permissionRepository.DeleteById(id, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Delete");
                return false;
            }

        }
        public PermissionItem GetById(int id)
        {
            try
            {
                return _permissionRepository.GetById(id, true, 72);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：GetById");
                return null;
            }
        }
        public bool HasPermissionByRole(int pid, string roleId)
        {
            try
            {
                return _permissionRepository.TableFormBuffer().Where(p => p.func == pid && p.roleId == roleId).Any();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByRole");
                return false;
            }
        }
        public bool HasPermissionByUser(int pid, string uid)
        {
            try
            {
                var roles = _appUserService.FindById(uid).Roles;
                foreach (var r in roles)
                {
                    if (HasPermissionByRole(pid, r.RoleId))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByUser");

            }
            return false;
        }
        public List<PermissionItem> GetPermissionByRole(string roleId)
        {
            try
            {
                return _permissionRepository.TableFormBuffer().Where(p => p.roleId == roleId).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByRole");
                return null;
            }
        }
        public List<PermissionItem> GetPermissionByUser(string uid)
        {
            var result = new List<PermissionItem>();
            try
            {
                var roles = _appUserService.FindById(uid).Roles;
                foreach (var r in roles)
                {
                    var rolesList = GetPermissionByRole(r.RoleId);
                    if (rolesList.Any())
                    {
                        foreach (var item in rolesList)
                        {
                            result.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByUser");
            }
            return result;
        }
        public List<NavigationViewModel> GetAll(string roleid)
        {
            try
            {
                var result = new List<NavigationViewModel>();
                var query = _navService.GetAll();
                var first = query.Where(s => s.level == 0 && s.pId == 0).ToList();
                for (var i = 0; i < first.Count(); i++)
                {
                    first[i].SonMenu = new List<NavigationViewModel>();
                    first[i].hasPermission = HasPermissionByRole(first[i].Id, roleid);
                    var second = query.Where(s => s.pId == first[i].Id).ToList();
                    if (second.Any())
                    {
                        for (var x = 0; x < second.Count; x++)
                        {
                            second[x].hasPermission = HasPermissionByRole(second[x].Id, roleid);
                            second[x].SonMenu = new List<NavigationViewModel>();
                            var third = query.Where(s => s.pId == second[x].Id).ToList();
                            if (third.Any())
                            {
                                for (var n = 0; n < third.Count; n++)
                                {
                                    third[n].hasPermission = HasPermissionByRole(second[x].Id, roleid);
                                    second[x].SonMenu.Add(third[n]);
                                }
                            }
                            first[i].SonMenu.Add(second[x]);
                        }
                    }
                    result.Add(first[i]);
                }
                return result.OrderBy(s => s.sequence).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetAll");
            }
            return new List<NavigationViewModel>();
        }
    }
}
