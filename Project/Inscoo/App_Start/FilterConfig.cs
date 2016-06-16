using Inscoo.Infrastructure;
using System.Web.Mvc;

namespace Inscoo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(DependencyResolver.Current.GetService<ExceptionFilter>());
        }
    }
}