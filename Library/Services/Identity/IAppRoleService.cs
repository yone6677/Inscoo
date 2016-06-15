using Core;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Identity
{
    public interface IAppRoleService
    {
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> DeleteAsync(AppRole role);
        Task<IdentityResult> UpdateAsync(AppRole role);
        Task<AppRole> FindByIdAsync(string roleid);
        Task<AppRole> FindByNameAsync(string name);
        Task<bool> RoleExistsAsync(string name);
        IQueryable<AppRole> Roles();
        List<SelectListItem> GetSelectList();
    }
}
