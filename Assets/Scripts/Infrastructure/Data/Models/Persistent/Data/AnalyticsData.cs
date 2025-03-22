namespace Infrastructure.Data.Models.Persistent.Data
{
    public class AnalyticsData
    {
        public int SessionsCount;

        public bool IsFirstSession => SessionsCount == 1;
    }
}