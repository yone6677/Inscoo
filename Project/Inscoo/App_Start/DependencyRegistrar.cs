using Autofac;
using Autofac.Integration.Mvc;
using Core.Data;
using Domain;
using Inscoo.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Services;
using Services.Common;
using Services.Identity;
using Services.Navigations;
using Services.Orders;
using Services.Permissions;
using Services.Products;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Services
{
    public class DependencyRegistrar
    {
        public static void RegisterDependency()
        {
            var builder = new ContainerBuilder();
            SetupResolveRules(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
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
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            //EF
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            //HTTP context and other related stuff
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerDependency();
            //global filter
            builder.RegisterFilterProvider();
            builder.RegisterType<ExceptionFilter>().InstancePerLifetimeScope();//异常过滤

            //baseServices
            builder.RegisterType<CachingManager>().As<ICachingManager>().InstancePerDependency();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerDependency();
            builder.RegisterType<FileService>().As<IFileService>().InstancePerDependency();
            builder.RegisterType<ResourceService>().As<IResourceService>().InstancePerDependency();
            builder.RegisterType<LoggerService>().As<ILoggerService>().InstancePerDependency();
            //services
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerDependency();
            builder.RegisterType<ArchiveService>().As<IArchiveService>().InstancePerDependency();
            builder.RegisterType<NavigationService>().As<INavigationService>().InstancePerDependency();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerDependency();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerDependency();
            builder.RegisterType<MixProductService>().As<IMixProductService>().InstancePerDependency();
            builder.RegisterType<MixProductItemService>().As<IMixProductItemService>().InstancePerDependency();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerDependency();
            builder.RegisterType<OrderItemService>().As<IOrderItemService>().InstancePerDependency();
            builder.RegisterType<OrderEmpService>().As<IOrderEmpService>().InstancePerDependency();
            builder.RegisterType<OrderBatchService>().As<IOrderBatchService>().InstancePerDependency();
            
        }
    }
}