using ChronoLog.Host.Configuration;
using System.Web.Http;
using Unity.WebApi;

namespace ChronoLog.Host.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            WebApiBootstrapper bootstrapper = new WebApiBootstrapper();
            bootstrapper.Run();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(bootstrapper.Container);
        }
    }
}