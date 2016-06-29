using Core.Pager;
using Domain;
using Models.Navigation;
using System.Collections.Generic;

namespace Services
{
    public interface INavigationService
    {
        bool Insert(Navigation item);
        bool Update(Navigation item);
        bool DeleteById(int id);
        Navigation GetById(int id);
        List<Navigation> GetLeftNavigations(string uId);
        List<Navigation> GetNotControllerNav();
        Navigation GetByUrl(string controller, string action);
        List<NavigationModel> GetSonViewList(int pid = 0);
        IPagedList<NavigationModel> GetList(int pageIndex = 1, int pageSize = 15, string name = null, int pId = 0, bool isShow = false, int level = 0);
        List<NavigationModel> GetAll();
    }
}
