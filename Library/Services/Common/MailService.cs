using System;
using System.Data.Entity;
using Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;

namespace Services
{
    public class MailService
    {
        static MailDbContext db;
        static MailDbContext Db
        {
            get
            {
                if (db == null)
                {
                    db = new MailDbContext();
                    return db;
                }
                else
                {
                    return db;
                }
            }
        }
        public MailService()
        {

        }

        public static int SendMail(MailQueue mailQueue)
        {
            var TestMail = ConfigurationManager.AppSettings["TestMail"].Trim();
            if (!string.IsNullOrEmpty(TestMail))
            {
                mailQueue.MQMAILBCC = "";
                mailQueue.MQMAILCC = "";
                mailQueue.MQMAILTO = TestMail;
            }
            return Db.SaveChanges();
        }
        public static async Task SendMailAsync(MailQueue mailQueue)
        {
            var TestMail = ConfigurationManager.AppSettings["TestMail"].Trim();
            if (!string.IsNullOrEmpty(TestMail))
            {
                mailQueue.MQMAILBCC = "";
                mailQueue.MQMAILCC = "";
                mailQueue.MQMAILTO = TestMail;
            }
            Db.MailQueues.Add(mailQueue);
            await Db.SaveChangesAsync();
        }
    }

    class MailDbContext : DbContext
    {
        public MailDbContext(string nameOrConnectionString = "MailLog")
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer(new AppDbInitializer());
            Database.SetInitializer<MailDbContext>(null);
            //Configuration.LazyLoadingEnabled = f;
        }

        public virtual IDbSet<MailQueue> MailQueues { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
