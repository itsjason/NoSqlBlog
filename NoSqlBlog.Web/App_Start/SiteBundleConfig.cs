using System.Web.Optimization;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(NoSqlBlog.Web.App_Start.SiteBundleConfig), "RegisterBundles")]

namespace NoSqlBlog.Web.App_Start
{
    public class SiteBundleConfig
    {
        public static void RegisterBundles()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jQuery").Include("~/Scripts/jQuery*"));
        }
    }
}