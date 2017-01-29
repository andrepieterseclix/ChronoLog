using Microsoft.Practices.Unity;

namespace CLog.Framework.Configuration.Bootstrap
{
    public interface IUnityDependencyInstaller
    {
        void Install(IUnityContainer container);
    }
}
