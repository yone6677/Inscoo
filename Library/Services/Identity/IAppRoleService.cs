using Domain;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public interface IAppRoleService
    {
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> DeleteAsync(AppRole role);
        SelectList GetRoleSelectList();
        Task<IdentityResult> UpdateAsync(AppRole role);
        AppRole FindByIdAsync(string roleid);
        Task<AppRole> FindByNameAsync(string name);
        AppRole FindByName(string name);
        Task<bool> RoleExistsAsync(string name);
        IQueryable<AppRole> Roles();
        List<SelectListItem> GetSelectList();
    }
}
