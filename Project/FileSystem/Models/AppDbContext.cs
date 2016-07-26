using System.Data.Entity;

namespace FileSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string nameOrConnectionString = "Inscoo")
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<AppDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArchiveMap());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ArchiveItem> archive { get; set; }
    }
}