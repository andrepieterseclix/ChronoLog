using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Automation;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class UpdateUserProfile : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_UpdateUserProfile()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            string newName = "Newname";
            string newSurname = "Newsurname";
            string newEmail = "new.email@company.co.za";

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);

            EnqueueChangeTab(set, USER_PROFILE_TAB_ID);

            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, NAME_INPUT_TEXT),
                nameText =>
                {
                    if (!nameText.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = (ValuePattern)nameText.GetCurrentPattern(ValuePattern.Pattern);
                    pattern.SetValue(newName);

                    return true;
                }));

            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, SURNAME_INPUT_TEXT),
                surnameText =>
                {
                    if (!surnameText.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = (ValuePattern)surnameText.GetCurrentPattern(ValuePattern.Pattern);
                    pattern.SetValue(newSurname);

                    return true;
                }));

            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, EMAIL_INPUT_TEXT),
                emailText =>
                {
                    if (!emailText.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = (ValuePattern)emailText.GetCurrentPattern(ValuePattern.Pattern);
                    pattern.SetValue(newEmail);

                    return true;
                }));

            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, SAVE_PROFILE_BUTTON),
                saveButton =>
                {
                    if (!saveButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)saveButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

                    return true;
                }));

            set.Enqueue(new AutomationAction(
                () =>
                {
                    AutomationElement mainWindow =
                        AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, MAIN_WINDOW_ID));

                    AutomationElement dialog =
                        mainWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Info"));

                    AutomationElement element =
                        dialog?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "OK"));

                    return element;
                },
                okButton =>
                {
                    if (!okButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)okButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

                    return true;
                }));

            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            DataTable userTable = GetData(string.Format(CultureInfo.CurrentCulture, "SELECT * FROM [dbo].[User] WHERE UserName = '{0}'", USER_NAME));
            Assert.IsNotNull(userTable);
            Assert.AreEqual(userTable.Rows.Count, 1);
            Assert.AreEqual(userTable.Rows[0]["Name"], newName);
            Assert.AreEqual(userTable.Rows[0]["Surname"], newSurname);
            Assert.AreEqual(userTable.Rows[0]["Email"], newEmail);
        }
    }
}
