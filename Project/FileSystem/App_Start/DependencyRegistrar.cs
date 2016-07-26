using Autofac;
using Autofac.Integration.WebApi;
using Core;
using Core.Data;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace FileSystem
{
    public class DependencyRegistrar
    {
        public static void RegisterDependency()
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();
            SetupResolveRules(builder);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;
        }
        private static void SetupResolveRules(ContainerBuilder builder)
        {
            //Order cannot be changed
            var conn = ConfigurationManager.ConnectionStrings["Inscoo"].ToString();
            //DbContext
            builder.Register<IdentityDbContext<AppUser>>(c => new AppDbContext(conn)).InstancePerDependency();
            //ASP.NET Identity
            builder.Register<IUserStore<AppUser>>(c => new UserStore<AppUser>(new AppDbContext(conn))).InstancePerDependency();
            builder.Register<IRoleStore<AppRole, string>>(c => new RoleStore<AppRole>(new AppDbContext(conn))).InstancePerDependency();
            builder.RegisterType<AppUserManager>();
            builder.RegisterType<AppRoleManager>();
            builder.RegisterType<AppUserService>().As<IAppUserService>().InstancePerDependency();
            builder.RegisterType<AppRoleService>().As<IAppRoleService>().InstancePerDependency();

            //EF
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            //HTTP context and other related stuff
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerDependency();

            //baseServices
            builder.RegisterType<CachingManager>().As<ICachingManager>().InstancePerDependency();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerDependency();
            builder.RegisterType<FileService>().As<IFileService>().InstancePerDependency();
            builder.RegisterType<ResourceService>().As<IResourceService>().InstancePerDependency();
            builder.RegisterType<LoggerService>().As<ILoggerService>().InstancePerDependency();

            builder.RegisterType<ArchiveService>().As<IArchiveService>().InstancePerDependency();
        }
        }
}