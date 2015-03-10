using TShockAPI;

namespace Herobrine
{
    public class Victim
    {
        public TSPlayer Player { get; set; }

        public Victim(TSPlayer player)
        {
            Player = player;
        }
    }
}