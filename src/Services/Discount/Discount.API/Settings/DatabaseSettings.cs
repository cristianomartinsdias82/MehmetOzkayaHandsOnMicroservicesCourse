namespace Discount.API.Settings
{
    public class DatabaseSettings
    {
        public string Server { get; init; }
        public string DatabaseName { get; init; }
        public string SchemaOwner { get; init; }
        public string TableName { get; init; }
        public string Port { get; init; }
        public string UserId { get; init; }
        public string Password { get; init; }
        public string ConnectionString { get; init; }
    }
}