namespace Discount.Grpc.Settings
{
    public static class ConnectionStringHelper
    {
        public static string BuildConnectionString(DatabaseSettings settings)
            => settings.ConnectionString
                .Replace("{SERVER}", settings.Server)
                .Replace("{PORT}", settings.Port)
                .Replace("{DATABASE}", settings.DatabaseName)
                .Replace("{USERID}", settings.UserId)
                .Replace("{PASSWORD}", settings.Password);
    }
}
