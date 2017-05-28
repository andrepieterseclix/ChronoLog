using System;

namespace CLog.Clients.IntegrationTests
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SkipTestAttribute : Attribute
    {
    }
}
