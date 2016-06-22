using Domain.Permission;
using Models.Navigation;
using System.Collections.Generic;

namespace Services.Permissions
{
    public interface IPermissionService
    {
        bool Insert(Permission item);
        bool Update(Permission item);
        bool Delete(Permission item);
        bool DeleteById(int id);
        Permission GetById(int id);
        bool HasPermissionByRoleId(int pid, string roleId);
        bool HasPermissionByRoleName(int pid, string roleName);
        bool HasPermissionByUser(int pid, string uid);
        List<Permission> GetPermissionByRole(string roleId);
        List<Permission> GetPermissionByUser(string uid);
        List<NavigationModel> GetAll(string roleid);
    }
}
