namespace CLog.UI.Framework.Testing.Models
{
    public sealed class TestModel
    {
        public TestModel()
        {
            Environment.Children.Add(new ServicesMockSettingsModel());
        }

        public GroupModel ViewModels { get; } = new GroupModel("View Models");

        public GroupModel Mocks { get; } = new GroupModel("Mocks");

        public GroupModel Environment { get; } = new GroupModel("Environment");
    }
}
