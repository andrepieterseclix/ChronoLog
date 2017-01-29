using CLog.Common.Logging;
using CLog.Framework.Security;
using CLog.Models.Mocks.Access;
using Moq;
using System;
using System.Threading;

namespace CLog.Business.Tests
{
    public abstract class ManagerTestBase
    {
        #region Fields

        protected readonly Mock<ILogger> _logger = new Mock<ILogger>();

        #endregion

        #region Constructors

        public ManagerTestBase()
        {
            ServerIdentity identity = new ServerIdentity(AccessDataHelper.USER_NAME1, 1, Guid.Parse("045a6bf3-12d6-485a-85e1-0bd750a9cdbd"), "Session Key", false, new string[0]);
            ServerPrincipal principal = new ServerPrincipal(identity);

            Thread.CurrentPrincipal = principal;
        }

        #endregion
    }
}
