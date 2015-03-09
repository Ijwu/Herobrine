using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Utils = TShockAPI.Utils;

namespace Herobrine
{
    public class Herobrine : TerrariaPlugin
    {
        public override string Name
        {
            get { return "Herobrine"; }
        }

        public override Version Version
        {
            get { return new Version(1,0,0); }
        }

        public override string Author
        {
            get { return "Iwju"; }
        }

        public override string Description
        {
            get { return "Allows you to haunt a player. Spoopy."; }
        }

        public Herobrine(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("herobrine.haunt", HauntPlayer, "haunt"));
        }

        private void HauntPlayer(CommandArgs args)
        {
            if (args.Parameters.Count > 0)
            {
                var plr = args.Parameters[0];
                var players = Utils.Instance.FindPlayer(plr);
                if (players.Count != 1)
                {
                    if (players.Count > 1)
                    {
                        args.Player.SendErrorMessage("Found multiple players {0}.", string.Join(", ", players));
                        return;
                    }
                    args.Player.SendErrorMessage("No player found by the name {0}.", plr);
                    return;
                }


            }
        }
    }
}
