using Core.Pager;
using Domain.Navigation;
using Models.Navigation;
using System.Collections.Generic;

namespace Services.Navigation
{
    public interface INavigationService
    {
        bool Insert(NavigationItem item);
        bool Update(NavigationItem item);
        bool Delete(NavigationItem item);
        NavigationItem GetById(int id);
        NavigationItem GetByUrl(string controller, string action);
        List<NavigationViewModel> GetSonViewList(int pid = 0);
        IPagedList<NavigationViewModel> GetList(int pageIndex = 1, int pageSize = 15, string name = null, int pId = 0, bool isShow = false, int level = 0);
        List<NavigationViewModel> GetAll();
    }
}
