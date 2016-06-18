using Core.Data;
using Domain.Permission;
using Microsoft.Owin.Security;
using Models.Navigation;
using Services.Identity;
using Services.Infrastructure;
using Services.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly INavigationService _navService;
        private readonly IAuthenticationManager _authenticationManager;
        public PermissionService(ILoggerService loggerService, IRepository<Permission> permissionRepository, IAppUserService appUserService, IAppRoleService appRoleService,
            INavigationService navService, IAuthenticationManager authenticationManager)
        {
            _loggerService = loggerService;
            _permissionRepository = permissionRepository;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
            _navService = navService;
            _authenticationManager = authenticationManager;
        }
        public bool Insert(Permission item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _permissionRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
                return false;
            }

        }
        public bool Update(Permission item)
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
        public bool Delete(Permission item)
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
        public Permission GetById(int id)
        {
            try
            {
                return _permissionRepository.GetById(id);
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
                return _permissionRepository.TableFromBuffer().Where(p => p.func == pid && p.roleId == roleId).Any();
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
        public List<Permission> GetPermissionByRole(string roleId)
        {
            try
            {
                return _permissionRepository.TableFromBuffer().Where(p => p.roleId == roleId).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByRole");
                return null;
            }
        }
        public List<Permission> GetPermissionByUser(string uid)
        {
            var result = new List<Permission>();
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
        public List<NavigationModel> GetAll(string roleid)
        {
            try
            {
                var result = new List<NavigationModel>();
                var query = _navService.GetAll();
                var first = query.Where(s => s.level == 0 && s.pId == 0).ToList();
                for (var i = 0; i < first.Count(); i++)
                {
                    first[i].SonMenu = new List<NavigationModel>();
                    first[i].hasPermission = HasPermissionByRole(first[i].Id, roleid);
                    var second = query.Where(s => s.pId == first[i].Id).ToList();
                    if (second.Any())
                    {
                        for (var x = 0; x < second.Count; x++)
                        {
                            second[x].hasPermission = HasPermissionByRole(second[x].Id, roleid);
                            second[x].SonMenu = new List<NavigationModel>();
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
            return new List<NavigationModel>();
        }
    }
}
