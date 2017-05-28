using CLog.Framework.ServiceClients;
using CLog.Framework.Services.Models;
using CLog.ServiceClients.Clients.Access;
using CLog.ServiceClients.Security;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using System;
using System.Globalization;
using System.Threading;

namespace CLog.Clients.IntegrationTests.ScriptedTests
{
    class AccessServiceTestDoubleLogout : ScriptedTest
    {
        private readonly AccessClientFactory factory = new AccessClientFactory();

        private IServiceClient<IAccessService> client;

        protected override void Setup()
        {
            client = factory.Create();
        }

        protected override void Teardown()
        {
            client?.Dispose();
        }

        protected override void Run()
        {
            // Log In
            LoginRequest request = new LoginRequest("Tester", "P@ssw0rd");
            LoginResponse response = client.Proxy.Login(request);

            if (response == null)
                throw new NullReferenceException();

            if (!response.IsLoggedIn)
                throw new Exception("Could not log in!");

            ClientPrincipal principal = Thread.CurrentPrincipal as ClientPrincipal;
            principal.Identity = new ClientIdentity(
                response.User.UserName,
                string.Format(CultureInfo.CurrentCulture, "{0} {1}", response.User.Name, response.User.Surname),
                response.Session.Id,
                response.Session.SessionKey,
                new string[0]);

            Console.WriteLine("Logged in:  {0}/{1}/{2}", response.User.UserName, response.Session.Id, response.Session.SessionKey);

            // Log Out
            LogoutRequest logoutRequest = new LogoutRequest(response.User.UserName, response.Session.Id, response.Session.SessionKey);
            LogoutResponse logoutResponse = client.Proxy.Logout(logoutRequest);

            if (logoutResponse.Errors.Count > 0)
                throw new Exception(string.Format("Could not log out:  {0}", logoutResponse.Errors[0].ToString()));

            Console.WriteLine("Logged out:  {0}", logoutRequest.SessionId);

            // Log Out 2nd time!
            logoutResponse = client.Proxy.Logout(logoutRequest);

            if (logoutResponse.Errors.Count > 0)
                throw new Exception(string.Format("Could not log out 2nd time:  {0}", logoutResponse.Errors[0].ToString()));

            Console.WriteLine("Logged out 2nd time:  {0}", logoutRequest.SessionId);
        }
    }
}
