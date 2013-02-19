using WebActivatorEx;
[assembly: PreApplicationStartMethod(typeof(NoSqlBlog.Web.App_Start.DependencyRegstrar), "WireDependencies")]

namespace NoSqlBlog.Web.App_Start
{
    using System.Web.Mvc;

    public static class DependencyRegstrar
    {
        public static void WireDependencies()
        {
            var container = new TinyIoC.TinyIoCContainer();
            DependencyResolver.SetResolver(container);
        }
    }
}