using Logger.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Logger.Models
{
    public class EfDbContext : DbContext
    {
        public EfDbContext()
           : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EfDbContext>());
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
       .Where(type => !String.IsNullOrEmpty(type.Namespace))
       .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
        public DbSet<Logs> log { get; set; }



    }
}