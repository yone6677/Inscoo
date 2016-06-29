using Domain;
using Models.Navigation;
using System.Collections.Generic;

namespace Services
{
    public interface IPermissionService
    {

        bool AddByUrl(string controller, string action, string roleId, string userName);
        bool Delete(Permission item);
        bool DeleteById(int id);
        bool DeleteByUrl(string controller, string action, string roleId);
        Permission GetById(int id);
        List<Permission> GetPermissionByRole(string roleId);
        List<Permission> GetPermissionByUser(string uid);
        List<NavigationModel> GetAll(string roleid);

        bool HasPermissionByRoleId(int navigationId, string roleId);
        bool HasPermissionByUser(int navigationId, string uid);
        bool HasNavUsedByRole(string controller, string action, string roleId);

        bool Insert(Permission item);
        bool Update(Permission item);

    }
}
