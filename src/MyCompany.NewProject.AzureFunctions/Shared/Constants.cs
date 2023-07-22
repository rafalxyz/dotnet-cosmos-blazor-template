namespace MyCompany.NewProject.AzureFunctions.Shared;

internal static class Constants
{
    internal static class Database
    {
        public const string ConnectionString = "ConnectionStrings:Database";
        public const string DatabaseName = "%Database:DatabaseName%";
        public const string ChangeFeedLeaseContainerName = "ChangeFeedLease";
        public const string DictionaryContainerName = "Dictionary";
    }

    internal static class AppConfig
    {
        public const string Endpoint = "AppConfig:Endpoint";
        public const string TestAppSettingsSentinel = "TestApp:Settings:Sentinel";
    }

    internal static class ServiceBus
    {
        public const string ConnectionString = "ConnectionStrings:ServiceBus";
    }
}