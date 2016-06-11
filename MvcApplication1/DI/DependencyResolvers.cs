using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace MvcApplication1
{
    public class DependencyResolvers : Idenpendency, IDependencyResolver
    {
        protected IUnityContainer container;

        public DependencyResolvers(IUnityContainer container)
        {
            this.container = container;
        }


        public void Dispose()
        {
            container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new DependencyResolvers(child);
        }
    }

    //
    class IOCContainer : DependencyResolvers
    {

        public IOCContainer(UnityContainer container)
            : base(container)
        {

        }

    }

}