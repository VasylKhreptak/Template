using Plugins.Banks.Integer;

namespace Infrastructure.Data.Models.Persistent.Data
{
    public class PlayerData
    {
        public readonly IntegerBank Coins = new IntegerBank(0);
    }
}