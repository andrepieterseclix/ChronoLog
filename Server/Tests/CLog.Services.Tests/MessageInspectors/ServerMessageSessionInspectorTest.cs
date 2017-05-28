using CLog.Business.Security.Contracts.Access;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Security;
using CLog.Framework.Services.Wcf.MessageInspectors;
using CLog.Models.Access;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace CLog.Services.Tests.MessageInspectors
{
    [TestClass]
    public class ServerMessageSessionInspectorTest
    {
        #region Fields

        private const string _action = "http://tempuri.org/ServiceName/MethodName";

        private readonly ServerMessageSessionInspector _inspector;

        private readonly Mock<Message> _message = new Mock<Message>();

        private readonly Mock<IClientChannel> _clientChannel = new Mock<IClientChannel>();

        private readonly Mock<IServiceLocator> _serviceLocator = new Mock<IServiceLocator>();

        private readonly Mock<ILogger> _logger = new Mock<ILogger>();

        private readonly Mock<IAccessManager> _accessManager = new Mock<IAccessManager>();

        private Func<string> _getCookie;

        #endregion

        #region Constructors

        public ServerMessageSessionInspectorTest()
        {
            _inspector = new ServerMessageSessionInspector(() => _action, GetCookie);

            // Setup service locator
            _serviceLocator
                .Setup(x => x.GetInstance<ILogger>())
                .Returns(_logger.Object);

            _serviceLocator
                .Setup(x => x.GetInstance<IAccessManager>(It.IsAny<string>()))
                .Returns(_accessManager.Object);

            ServiceLocator.SetLocatorProvider(() => _serviceLocator.Object);
        }

        #endregion

        #region Helper Methods

        private string GetCookie()
        {
            return _getCookie?.Invoke();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        public void ServerMessageSessionInspector_AfterReceiveRequest_Anonymous_Success()
        {
            // Arrange
            Message request = _message.Object;

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));

            // Assert
            Assert.IsTrue(Thread.CurrentPrincipal.Identity.GetType() == typeof(AnonymousServerIdentity));
        }

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        [ExpectedException(typeof(SecurityException))]
        public void ServerMessageSessionInspector_AfterReceiveRequest_InvalidCookie_Failure()
        {
            // Arrange
            Message request = _message.Object;
            _getCookie = () => "InvalidCookieValue";

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));
        }

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        [ExpectedException(typeof(SecurityException))]
        public void ServerMessageSessionInspector_AfterReceiveRequest_InvalidGuid_Failure()
        {
            // Arrange
            Message request = _message.Object;
            _getCookie = () => "UserName/[Guid]/SessionKey";

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));
        }

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        [ExpectedException(typeof(SecurityException))]
        public void ServerMessageSessionInspector_AfterReceiveRequest_InvalidSession_Failure()
        {
            // Arrange
            Message request = _message.Object;
            _getCookie = () => "UserName/2c3f0541-6fd6-4105-b4a2-806059a26631/SessionKey";

            SessionState sessionState = new SessionState()
            {
                IsExpired = false,
                UserId = 1
            };

            BusinessResult<SessionState> result = new BusinessResult<SessionState>(sessionState);
            result.Errors.Add(new ErrorMessage("Code", "Message"));

            _accessManager
                .Setup(x => x.ValidateSession(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(result);

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));
        }

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        public void ServerMessageSessionInspector_AfterReceiveRequest_ExpiredSession_Success()
        {
            // Arrange
            Message request = _message.Object;
            _getCookie = () => "UserName/2c3f0541-6fd6-4105-b4a2-806059a26631/SessionKey";

            SessionState sessionState = new SessionState()
            {
                IsExpired = true,
                UserId = 1
            };

            BusinessResult<SessionState> result = new BusinessResult<SessionState>(sessionState);

            _accessManager
                .Setup(x => x.ValidateSession(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(result);

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));

            // Assert
            ServerPrincipal principal = Thread.CurrentPrincipal as ServerPrincipal;

            Assert.IsNotNull(principal);
            Assert.IsTrue(principal.Identity.SessionExpired);
        }

        [TestMethod]
        [TestCategory("Services - Message Inspectors - Access")]
        public void ServerMessageSessionInspector_AfterReceiveRequest_Success()
        {
            // Arrange
            Message request = _message.Object;
            string userName = "UserName";
            _getCookie = () => string.Format(CultureInfo.CurrentCulture, "{0}/2c3f0541-6fd6-4105-b4a2-806059a26631/SessionKey", userName);

            SessionState sessionState = new SessionState()
            {
                IsExpired = false,
                UserId = 1
            };

            BusinessResult<SessionState> result = new BusinessResult<SessionState>(sessionState);

            _accessManager
                .Setup(x => x.ValidateSession(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(result);

            // Act
            _inspector.AfterReceiveRequest(ref request, _clientChannel.Object, new InstanceContext(new object()));

            // Assert
            ServerPrincipal principal = Thread.CurrentPrincipal as ServerPrincipal;

            Assert.IsNotNull(principal);
            Assert.IsFalse(principal.Identity.SessionExpired);
            Assert.IsFalse(Thread.CurrentPrincipal.Identity.GetType() == typeof(AnonymousServerIdentity));
            Assert.IsTrue(Thread.CurrentPrincipal.Identity.GetType() == typeof(ServerIdentity));
            Assert.IsTrue(Thread.CurrentPrincipal.Identity.IsAuthenticated);
            Assert.IsTrue(Thread.CurrentPrincipal.Identity.Name == userName);
        }

        #endregion
    }
}
