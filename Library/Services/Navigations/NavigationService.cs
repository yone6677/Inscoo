using Core.Data;
using Core.Pager;
using Domain.Navigation;
using Microsoft.Owin.Security;
using Models.Navigation;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Navigations
{
    public class NavigationService : INavigationService
    {
        private readonly IRepository<Navigation> _navRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public NavigationService(IRepository<Navigation> navRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _navRepository = navRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
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

        public Navigation GetByUrl(string controller, string action)
        {
            try
            {
                return _navRepository.TableFromBuffer(72).Where(s => s.controller == controller && s.action == action).FirstOrDefault();
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
                        pId = s.pId,
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
        public IPagedList<NavigationModel> GetList(int pageIndex, int pageSize, string name, int pId, bool isShow, int level)
        {
            try
            {
                var query = _navRepository.TableFromBuffer(72);
                if (query != null)
                {
                    query = query.Where(q => q.pId == pId);
                    if (level > 0)
                    {
                        query = query.Where(q => q.level == level);
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(q => q.name == name);
                    }
                    if (isShow)
                    {
                        query = query.Where(q => q.isShow == true);
                    }
                    query = query.OrderBy(q => q.sequence);
                    return new PagedList<NavigationModel>(query.Select(s => new NavigationModel()
                    {
                        Id = s.Id,
                        action = s.action,
                        controller = s.controller,
                        isShow = s.isShow,
                        level = s.level,
                        memo = s.memo,
                        name = s.name,
                        pId = s.pId,
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
                    pId = s.pId,
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
