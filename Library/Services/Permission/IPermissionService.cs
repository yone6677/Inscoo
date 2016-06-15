using Domain.Permission;
using Models.Navigation;
using System.Collections.Generic;

namespace Services.Permission
{
    public interface IPermissionService
    {
        bool Insert(PermissionItem item);
        bool Update(PermissionItem item);
        bool Delete(PermissionItem item);
        bool DeleteById(int id);
        PermissionItem GetById(int id);
        bool HasPermissionByRole(int pid, string roleId);
        bool HasPermissionByUser(int pid, string uid);
        List<PermissionItem> GetPermissionByRole(string roleId);
        List<PermissionItem> GetPermissionByUser(string uid);
        List<NavigationViewModel> GetAll(string roleid);
    }
}
