using System.Collections.Generic;
using System.Linq;
using Herobrine.Attributes;
using Herobrine.Concrete.WorldEdits;
using Terraria;

namespace Herobrine.Concrete.Hauntings
{
    [Haunting("LightsOut", "lightsout")]
    class LightsOutHaunting : BaseHaunting
    {
        private static readonly int[] SwitchableLightIds =
        {
            92,
            42,
            100,
            34,
            33,
            93,
            95,
            4,
            126,
            149
        };
        public LightsOutHaunting(Victim victim) : base(victim)
        {
            
        }

        public override void Update()
        {
            var tilesInSquare = Victim.GetTilesInSquare(20);
            foreach (var point in tilesInSquare)
            {
                var tile = Main.tile[point.X, point.Y];
                if (SwitchableLightIds.Contains(tile.type))
                {
                    
                    if (tile.inActive())
                    {
                        MakeEdit(new SwitchEdit(point.X, point.Y));   
                    }
                }
            }
        }
    }
}