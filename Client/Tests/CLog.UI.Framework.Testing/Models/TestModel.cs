namespace CLog.UI.Framework.Testing.Models
{
    public sealed class TestModel
    {
        public GroupModel ViewModels { get; } = new GroupModel("View Models");

        public GroupModel Mocks { get; } = new GroupModel("Mocks");

        public GroupModel Environment { get; } = new GroupModel("Environment");
    }
}
