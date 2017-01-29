using CLog.Framework.ServiceClients;
using CLog.Framework.Services.Models;
using CLog.ServiceClients.Clients.Access;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using System;

namespace CLog.Clients.IntegrationTests.ScriptedTests
{
    class AccessServiceTesUserSuspended : ScriptedTest
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
            LoginRequest request = new LoginRequest("AP1204", "password");
            LoginResponse response = client.Proxy.Login(request);

            if (response == null)
                throw new NullReferenceException();

            foreach (ErrorDto error in response.Errors)
            {
                Console.WriteLine("Response Error:  ({0}) {1}", error.Code, error.Message);
                if (!string.IsNullOrWhiteSpace(error.AdditionalInfo))
                    Console.WriteLine("\t{0}", error.AdditionalInfo);
            }

            Console.WriteLine("Is Logged In:  {0}", response.IsLoggedIn);
            if (response.User != null)
                Console.WriteLine("({0}) {1} {2}:  {3}", response.User.UserName, response.User.Name, response.User.Surname, response.User.Email);
            if (response.Session != null)
                Console.WriteLine("Session ({0}):  {1}", response.Session.Id, response.Session.SessionKey);
        }
    }
}
