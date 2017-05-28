using CLog.ServiceClients.Security;
using System;

namespace CLog.Clients.IntegrationTests
{
    abstract class ScriptedTest
    {
        public void RunTest()
        {
            try
            {
                AppDomain.CurrentDomain.SetThreadPrincipal(new ClientPrincipal());

                Setup();
                Console.WriteLine("\r\n-- Running {0} --", GetType().Name);
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ExceptionThrown?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                Teardown();
            }
        }

        public event EventHandler ExceptionThrown;

        protected virtual void Setup()
        {
        }

        protected virtual void Teardown()
        {
        }

        protected abstract void Run();
    }
}
