namespace dev.codingWombat.Vombatidae.config
{
    public class CacheConfiguration
    {
        public const string Configuration = "Cache";
        public bool UseReddis { get; set; }
        public string Host { get; set; }
        public string Instance { get; set; }
    }
}