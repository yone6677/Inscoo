using Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
namespace Core.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(string nameOrConnectionString = "Inscoo")
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new AppDbInitializer());
            Configuration.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());
            //此段作用是遍历所有继承 EntityTypeConfiguration 的映射类
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //modelBuilder.Configurations.Add(new MemberShipMap()); 也可以使用此方法实现单例映射
            base.OnModelCreating(modelBuilder);
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}
