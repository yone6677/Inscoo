using Core.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Identity
{
    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            new initializer(context).InitUserAndRole();
            base.Seed(context);
        }
    }
    class initializer
    {
        IdentityDbContext<AppUser> _context;
        public initializer(IdentityDbContext<AppUser> context)
        {
            _context = context;
        }
        public void InitUserAndRole()
        {
            //添加admin用户，密码123456
            var user = new AppUser() { UserName = "Admin", CreaterId = "", ModifierId = "", PasswordHash = "ABo9ONAMgkexrgRTN919lzfKw74MNsiN7kkkf6IOc/ZsVvewJGIohiZsL8nIqZ4/5w==", SecurityStamp = "4c773bd1-61ba-4d60-ae19-97dfbdae46f4", Email = "1172445486@qq.com", CompanyName = "Inscoo" };
            _context.Users.Add(user);

            var adminRole = new AppRole { Name = "Admin", Description = "Admin" };
            _context.Roles.Add(adminRole);
            _context.Roles.Add(new AppRole { Name = "BusinessDeveloper", Description = "BusinessDeveloper" });
            _context.Roles.Add(new AppRole { Name = "Channel", Description = "Channel" });
            _context.Roles.Add(new AppRole { Name = "CompanyHR", Description = "CompanyHR" });
            _context.Roles.Add(new AppRole { Name = "InscooFinance", Description = "InscooFinance" });
            _context.Roles.Add(new AppRole { Name = "InsuranceCompany", Description = "保险公司" });
            _context.SaveChanges();
            user.CreaterId = user.Id;
            user.ModifierId = user.ModifierId;

            var sqlAddAdminRoleForAdmin = string.Format("insert into [AspNetUserRoles] values('{0}','{1}','IdentityUserRole')", user.Id, adminRole.Id);
            _context.Database.ExecuteSqlCommand(sqlAddAdminRoleForAdmin);
            _context.SaveChanges();

        }
        //public void InitializeIdentityForEF()
        //{
        //    var _userManager = AppUserManager.Create();
        //    var _roleManager = AppRoleManager.Create();
        //    string name = "admin";
        //    string roleName = "Admin";
        //    //如果没有Admin用户组则创建该组
        //    var role = _roleManager.FindByName(roleName);
        //    if (role == null)
        //    {
        //        role = new AppRole() { Name = roleName, Description = "管理员" };
        //        var roleresult = _roleManager.Create(role);
        //    }
        //    var user = _userManager.FindByName(name);
        //    if (user == null)
        //    {
        //        user = new AppUser()
        //        {
        //            UserName = name,
        //            Email = "admin@Acer.com",
        //            PhoneNumber = "13572408176",
        //            LinkMan = "Leo",
        //            CompanyName = "Acer",
        //        };
        //        _userManager.Create(user, "123456");
        //        _userManager.SetLockoutEnabled(user.Id, false);
        //    }
        //    // 把用户admin添加到用户组Admin中
        //    var rolesForUser = _userManager.GetRoles(user.Id);
        //    if (!rolesForUser.Contains(role.Name))
        //    {
        //        var result = _userManager.AddToRole(user.Id, role.Name);
        //    }
        //}
    }
}
