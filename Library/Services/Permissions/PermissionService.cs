using Core.Data;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Navigation;
using Services;
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
        public bool DeleteByUrl(string controller, string action, string roleId)
        {
            try
            {
                if (controller == "控制器以外")
                {
                    var navs = _navService.GetNotControllerNav();
                    var nav = navs.Single(s => s.name.Equals(action) && string.IsNullOrEmpty(s.controller));
                    var per = _permissionRepository.Table.SingleOrDefault(p => p.roleId == roleId && p.NavigationId == nav.Id);
                    if (per != null)
                    {
                        _permissionRepository.Delete(per, disable: true);
                        return true;
                    }
                    return false;
                }
                else
                {
                    var nav = _navService.GetByUrl(controller, action);
                    if (nav != null)
                    {
                        var per = _permissionRepository.Table.SingleOrDefault(p => p.roleId == roleId && p.NavigationId == nav.Id);
                        if (per != null)
                        {
                            _permissionRepository.Delete(per, disable: true);
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Delete");
                return false;
            }

        }
        public bool AddByUrl(string controller, string action, string roleId, string userName)
        {
            try
            {
                if (controller == "控制器以外")
                {
                    var navs = _navService.GetNotControllerNav();
                    var nav = navs.Single(s => s.name.Equals(action) && string.IsNullOrEmpty(s.controller));
                    return Insert(new Permission() { Author = userName, NavigationId = nav.Id, roleId = roleId });
                }
                else
                {
                    var nav = _navService.GetByUrl(controller, action);
                    if (nav == null)
                    {
                        var newNav = new Navigation()
                        {
                            action = action,
                            controller = controller,
                            url = controller.Substring(0, controller.Length - 10) + "/" + action,
                            pId = 0,
                            isShow = false,
                            level = 2,
                            name = action
                        };
                        var insertResult = _navService.Insert(newNav);
                        if (insertResult)
                        {
                            return Insert(new Permission() { Author = userName, NavigationId = newNav.Id, roleId = roleId });
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var isExist = HasPermissionByRoleId(nav.Id, roleId);
                        if (isExist) return true;
                        return Insert(new Permission() { Author = userName, NavigationId = nav.Id, roleId = roleId });
                    }
                }
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
                    first[i].hasPermission = HasPermissionByRoleId(first[i].Id, roleid);
                    var second = query.Where(s => s.pId == first[i].Id).ToList();
                    if (second.Any())
                    {
                        for (var x = 0; x < second.Count; x++)
                        {
                            second[x].hasPermission = HasPermissionByRoleId(second[x].Id, roleid);
                            second[x].SonMenu = new List<NavigationModel>();
                            var third = query.Where(s => s.pId == second[x].Id).ToList();
                            if (third.Any())
                            {
                                for (var n = 0; n < third.Count; n++)
                                {
                                    third[n].hasPermission = HasPermissionByRoleId(second[x].Id, roleid);
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

        //public List<Permission> GetAllAction(string roleid)
        //{
        //    var controls=Basecon


        //}

        public bool HasPermissionByRoleId(int navigationId, string roleId)
        {
            try
            {
                return _permissionRepository.TableFromBuffer().Where(p => p.NavigationId == navigationId && p.roleId == roleId).Any();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByRole");
                return false;
            }
        }

        public bool HasPermissionByUser(int navigationId, string uid)
        {
            try
            {
                var roles = _appUserService.FindById(uid).Roles;
                if (roles.Any(r => r.RoleId == "70e917dc-a514-45ea-93a5-4f56343e9e10")) return true;

                return roles.Any(r => HasPermissionByRoleId(navigationId, r.RoleId));

            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByUser");

            }
            return false;
        }
        public bool HasNavUsedByRole(string controller, string action, string roleId)
        {
            try
            {
                var nav = _navService.GetByUrl(controller, action);
                if (nav == null) return false;
                else
                {
                    return HasPermissionByRoleId(nav.Id, roleId);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：HasPermissionByRole");
                return false;
            }
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


    }
}
