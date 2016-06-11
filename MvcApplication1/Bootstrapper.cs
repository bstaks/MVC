using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using MvcApplication1.DataModels;
using MvcApplication1.Controllers;
using MvcApplication1.Servicelocator;
using System.Web.Http;

namespace MvcApplication1
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>(); 
            //container.RegisterType<IdbServices, ShoppingCart>();
            container.RegisterInstance(new ShoppingCart());
            container.RegisterType<AdminController>();
          //  GlobalConfiguration.Configuration.DependencyResolver = new IOCContainer(container);


            return container;
        }
    }
}