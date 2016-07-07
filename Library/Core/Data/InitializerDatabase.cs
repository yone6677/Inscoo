using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Domain;

namespace Core.Data
{
    public class InitializerDatabase
    {
        private IdentityDbContext<AppUser> _context;
        public InitializerDatabase(IdentityDbContext<AppUser> context)
        {
            _context = context;
            InitBaseData();
            InitUserAndRoles();

        }
        private void InitUserAndRoles()
        {
            //添加admin用户，密码123456
            var user = new AppUser() { UserName = "Admin", CreaterId = "", Changer = "", PasswordHash = "ABo9ONAMgkexrgRTN919lzfKw74MNsiN7kkkf6IOc/ZsVvewJGIohiZsL8nIqZ4/5w==", SecurityStamp = "4c773bd1-61ba-4d60-ae19-97dfbdae46f4", Email = "1172445486@qq.com", CompanyName = "Inscoo",ProdInsurance = "",ProdSeries = ""};
            _context.Users.Add(user);
            //var admin = new AppRole { Name = "Admin", Description = "Admin" };
            //_context.Roles.Add(admin);
            //_context.Roles.Add(new AppRole { Name = "BusinessDeveloper", Description = "BusinessDeveloper" });
            //_context.Roles.Add(new AppRole { Name = "PartnerChannel", Description = "PartnerChannel" });
            //_context.Roles.Add(new AppRole { Name = "CompanyHR", Description = "CompanyHR" });
            //_context.Roles.Add(new AppRole { Name = "InscooFinance", Description = "InscooFinance" });
            //_context.Roles.Add(new AppRole { Name = "InsuranceCompany", Description = "保险公司" });
            _context.SaveChanges();
            user.CreaterId = user.Id;
            user.Changer = user.Id;

            var sqlAddAdminRoleForAdmin = string.Format("insert into [AspNetUserRoles] values('{0}','{1}','{2}')", user.Id, "70e917dc-a514-45ea-93a5-4f56343e9e10", "IdentityUserRole");
            _context.Database.ExecuteSqlCommand(sqlAddAdminRoleForAdmin);
            _context.SaveChanges();
        }
        private void InitBaseData()
        {
            try
            {

                var customCommands = new List<string>();
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/Role.sql"), false));//产品
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/Products.sql"), false));//产品
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/MixProduct.sql"), false));//推荐产品
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/GenericAttribute.sql"), false));//通用属性         
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/menu.sql"), false));//菜单
                customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Sql/Permission.sql"), false));//权限
                if (customCommands.Count <= 0) return;
                foreach (var command in customCommands)
                {
                    _context.Database.ExecuteSqlCommand(command);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #region Utilities

        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));

                return new string[0];
            }


            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion
    }
}
