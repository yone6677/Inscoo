using Core.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Core.Identity
{
    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            new InitializerDatabase(context);
            base.Seed(context);
        }
    }
}
