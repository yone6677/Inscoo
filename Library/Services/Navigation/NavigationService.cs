using Core.Data;
using Core.Pager;
using Domain.Navigation;
using Models.Navigation;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<NavigationItem> _navRepository;
        public NavigationService(ILoggerService loggerService, IRepository<NavigationItem> navRepository)
        {
            _loggerService = loggerService;
            _navRepository = navRepository;
        }
        public bool Insert(NavigationItem item)
        {
            try
            {
                _navRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：Insert");
                return false;
            }

        }

        public bool Update(NavigationItem item)
        {
            try
            {
                _navRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：Update");
                return false;
            }

        }

        public bool Delete(NavigationItem item)
        {
            try
            {
                _navRepository.Delete(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：Delete");
                return false;
            }

        }

        public NavigationItem GetById(int id)
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
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetById");
                return null;
            }
        }

        public NavigationItem GetByUrl(string controller, string action)
        {
            try
            {
                return _navRepository.Table.Where(s => s.controller == controller && s.action == action).FirstOrDefault();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetByUrl");
                return null;
            }

        }
        public List<NavigationItem> GetSonEnitityList(int pid)
        {
            try
            {
                if (pid > 0)
                {
                    return _navRepository.Table.Where(s => s.pId == pid).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetSonList");
            }
            return null;
        }
        public List<NavigationViewModel> GetSonViewList(int pid)
        {
            try
            {
                if (pid > 0)
                {
                    return _navRepository.Table.Where(s => s.pId == pid).Select(s => new NavigationViewModel()
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
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetSonList");
            }
            return null;
        }
        public IPagedList<NavigationViewModel> GetList(int pageIndex, int pageSize, string name, int pId, bool isShow, int level)
        {
            try
            {
                var query = _navRepository.Table;
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
                query = query.OrderByDescending(q => q.Id);
                return new PagedList<NavigationViewModel>(query.Select(s => new NavigationViewModel()
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
                    sequence=s.sequence
                }), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "系统功能：GetListOfPager");
                return null;
            }
        }
        public List<NavigationViewModel> GetAll()
        {
            try
            {
                return _navRepository.Table.Select(s => new NavigationViewModel()
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
                    sequence=s.sequence
                }).ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetAll");
                return null;
            }
        }
    }
}
