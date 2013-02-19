using WebActivatorEx;
[assembly: PreApplicationStartMethod(typeof(NoSqlBlog.Web.App_Start.DependencyRegstrar), "WireDependencies")]

namespace NoSqlBlog.Web.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Core.Interfaces;
    using Services;
    using TinyIoC;

    public static class DependencyRegstrar
    {
        public static void WireDependencies()
        {
            DependencyResolver.SetResolver(new TinyLocator());
        }
    }

    public class TinyLocator : IDependencyResolver //Microsoft.Practices.ServiceLocation.IServiceLocator
    {
        private readonly TinyIoCContainer _container;

        public TinyLocator()
        {
            _container = new TinyIoC.TinyIoCContainer();
            _container.Register<IPostRepository, RavenPostRepository>();
        }

        public object GetService(Type serviceType)
        {
            if (_container.CanResolve(serviceType))
                return _container.Resolve(serviceType);

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }
    }
}