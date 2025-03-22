namespace Infrastructure.Data.Models.Persistent.Data.Core
{
    public class PersistentData
    {
        public readonly PlayerData PlayerData = new PlayerData();
        public readonly AnalyticsData AnalyticsData = new AnalyticsData();
        public readonly SettingsData SettingsData = new SettingsData();
    }
}