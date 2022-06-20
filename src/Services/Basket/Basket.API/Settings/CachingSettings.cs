namespace Basket.API.Settings
{
    public class CachingSettings
    {
        public string ServerAddress { get; init; }
        public int SlidingExpirationTTLInMinutes { get; init; }
    }
}
