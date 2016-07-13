using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Linq;

namespace Domain
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<AppUser>(this)
            {
                //todo:尚不能唯一邮件，没有测试
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false,


            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 8,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
           
            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = false;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 20;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            this.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<AppUser>
            {
                MessageFormat = "您的安全码: {0}"
            });


            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<AppUser>
            {
                Subject = "邮箱确认",
                BodyFormat = "<a href=‘Login.aspx? {0}’>点击</a>确认"

            });
            var provider = new DpapiDataProtectionProvider();
            this.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(provider.Create("UserToken"));
            this.EmailService = new EmailService();

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    this.UserTokenProvider =
            //        new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}

        }
    }
}
