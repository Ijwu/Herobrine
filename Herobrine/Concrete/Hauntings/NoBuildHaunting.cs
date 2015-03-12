using System.IO;
using System.Linq;
using Herobrine.Attributes;
using TerrariaApi.Server;

namespace Herobrine.Concrete.Hauntings
{
    [HauntingItemDescription("NoBuild", "Randomly breaks blocks that the player places.","nobuild")]
    public class NoBuildHaunting : BaseHaunting
    {
        public NoBuildHaunting(Victim victim) : base(victim)
        {
            var plugin = ServerApi.Plugins.Single(x => x.Plugin.Name == "Herobrine").Plugin;
            ServerApi.Hooks.NetGetData.Register(plugin, GetTilePacket);
        }

        private void GetTilePacket(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.Tile)
            {
                var action = args.Msg.binaryReader.ReadByte();
                var tilex = args.Msg.binaryReader.ReadInt16();
                var tiley = args.Msg.binaryReader.ReadInt16();
                Herobrine.Debug("NoBuild: Tile edit action {0} at ({1}, {2})", action, tilex, tiley);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void CleanUp()
        {
            var plugin = ServerApi.Plugins.Single(x => x.Plugin.Name == "Herobrine").Plugin;
            ServerApi.Hooks.NetGetData.Deregister(plugin, GetTilePacket);
            base.CleanUp();
        }
    }
}