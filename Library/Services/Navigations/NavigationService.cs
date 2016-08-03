using Core.Data;
using Core.Pager;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;

namespace Services
{
    public class NavigationService : INavigationService
    {
        private readonly IRepository<Navigation> _navRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        private readonly IAppUserService _svAppUser;
        private readonly IAppRoleService _svAppRole;
        public NavigationService(IRepository<Navigation> navRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService, IAppUserService svAppUser, IAppRoleService svAppRole)
        {
            _navRepository = navRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
            _svAppUser = svAppUser;
            _svAppRole = svAppRole;
        }
        public bool Insert(Navigation item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _navRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：Insert");
                return false;
            }

        }

        public bool Update(Navigation item)
        {
            try
            {
                _navRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：Update");
                return false;
            }

        }

        public bool DeleteById(int id)
        {
            try
            {
                _navRepository.DeleteById(id, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：Delete");
                return false;
            }

        }

        public Navigation GetById(int id)
        {
            try
            {
                var item = _navRepository.GetById(id);
                if (item != null)
                {
                    item.SonMenu = GetSonEnitityList(item.Id);
                }
                return item;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetById");
                return null;
            }
        }
        public List<Navigation> GetNotControllerNav()
        {
            try
            {
                return _navRepository.TableFromBuffer().ToList().Where(n => string.IsNullOrEmpty(n.controller)).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetById");
                return null;
            }
        }
        public List<Navigation> GetLeftNavigations(string uId)
        {
            try
            {
                var roles = _svAppUser.GetRolesByUserId(uId);
                if (roles.Any(r => r.Equals("admin", StringComparison.CurrentCultureIgnoreCase)))
                {
                    var navs = _navRepository.TableFromBuffer().ToList().Where(p => p.isShow);

                    return navs.OrderBy(n => n.sequence).ToList();
                }
                else if (roles.Count == 1 && roles.Contains("InscooOperator"))
                {
                    var navs = _navRepository.TableFromBuffer().Where(p => p.isShow).ToList();
                    var removeItems = navs.Where(n => n.controller != null).Where(n => n.controller.Equals("RoleController", StringComparison.CurrentCultureIgnoreCase) || n.controller.Equals("NavController", StringComparison.CurrentCultureIgnoreCase) || n.controller.Equals("PermissionController", StringComparison.CurrentCultureIgnoreCase) || n.controller.Equals("GenericattributeController", StringComparison.CurrentCultureIgnoreCase));
                    if (removeItems.Any())
                    {
                        foreach (var item in removeItems.ToList())
                        {
                            navs.Remove(item);
                        }
                    }
                    return navs.OrderBy(n => n.sequence).ToList();
                }
                else
                {
                    var roleIds = _svAppRole.Roles().Where(r => roles.Contains(r.Name)).Select(r => r.Id).AsNoTracking().ToList();
                    var navs = from p in _navRepository.DatabaseContext.Set<Permission>().Include(n => n.Navigation).AsNoTracking()
                               let n = p.Navigation
                               where (roleIds.Contains(p.roleId) && p.Navigation.isShow)
                               select (n);

                    return navs.OrderBy(n => n.sequence).ToList();
                }

            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetById");
                return new List<Navigation>();
            }
        }
        public Navigation GetByUrl(string controller, string action)
        {
            try
            {
                return _navRepository.TableFromBuffer(72).Where(s => controller.Equals(s.controller, StringComparison.CurrentCultureIgnoreCase) && action.Equals(s.action, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetByUrl");
                return null;
            }

        }
        public List<Navigation> GetSonEnitityList(int pid)
        {
            try
            {
                if (pid > 0)
                {
                    return _navRepository.TableFromBuffer(72).Where(s => s.pId == pid).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetSonList");
            }
            return null;
        }
        public List<NavigationModel> GetSonViewList(int pid)
        {
            try
            {
                if (pid > 0)
                {
                    return _navRepository.TableFromBuffer(72).Where(s => s.pId == pid).Select(s => new NavigationModel()
                    {
                        action = s.action,
                        controller = s.controller,
                        isShow = s.isShow,
                        level = s.level,
                        memo = s.memo,
                        name = s.name,
                        PId = s.pId,
                        url = s.url,
                        htmlAtt = s.htmlAtt
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetSonList");
            }
            return null;
        }
        public SelectList GetParentNavList(int selectedPId)
        {
            try
            {

                var list = new List<Navigation>();
                list.Add(new Navigation() { name = "父菜单", Id = 0 });
                list.AddRange(_navRepository.TableFromBuffer(72).Where(s => s.isShow && s.pId == 0 && !s.IsDeleted).OrderBy(s => s.sequence));
                if (list.Any()) return new SelectList(list.ToList(), "Id", "name", selectedPId);

            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetParentNavList");
            }
            return new SelectList(new List<SelectListItem>());
        }
        public IPagedList<NavigationModel> GetList(int pageIndex, int pageSize, int pId)
        {
            try
            {
                var query = _navRepository.TableFromBuffer(72);
                if (query != null)
                {
                    query = query.Where(q => q.pId == pId && q.isShow && !q.IsDeleted).OrderBy(q => q.sequence);
                    //if (level > 0)
                    //{
                    //    query = query.Where(q => q.level == level);
                    //}
                    //if (!string.IsNullOrEmpty(name))
                    //{
                    //    query = query.Where(q => q.name == name);
                    //}
                    //if (isShow)
                    //{
                    //    query = query.Where(q => q.isShow == true);
                    //}
                    return new PagedList<NavigationModel>(query.Select(s => new NavigationModel()
                    {
                        Id = s.Id,
                        action = s.action,
                        controller = s.controller,
                        isShow = s.isShow,
                        level = s.level,
                        memo = s.memo,
                        name = s.name,
                        PId = s.pId,
                        url = s.url,
                        htmlAtt = s.htmlAtt,
                        sequence = s.sequence
                    }), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new PagedList<NavigationModel>(new List<NavigationModel>(), pageIndex, pageSize);
        }
        public List<NavigationModel> GetAll()
        {
            try
            {
                return _navRepository.TableFromBuffer(72).Select(s => new NavigationModel()
                {
                    Id = s.Id,
                    action = s.action,
                    controller = s.controller,
                    isShow = s.isShow,
                    level = s.level,
                    memo = s.memo,
                    name = s.name,
                    PId = s.pId,
                    url = s.url,
                    htmlAtt = s.htmlAtt,
                    sequence = s.sequence
                }).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetAll");
                return new List<NavigationModel>();
            }
        }
    }
}
