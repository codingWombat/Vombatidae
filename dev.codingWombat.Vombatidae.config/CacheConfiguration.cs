namespace dev.codingWombat.Vombatidae.config
{
    public class CacheConfiguration
    {
        public const string Configuration = "Cache";
        public bool UseRedis { get; set; }
        public string Host { get; set; }
        public string Instance { get; set; }
        public int SlidingExpiration { get; set; }
    }
}