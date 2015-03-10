using System;
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

        private static readonly short[] TurnedOffFrameX =
        {
            18,
            18,
            36,
            54,
            18,
            18,
            36,
            54,
            36,
            54
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
                    var index = Array.IndexOf(SwitchableLightIds, tile.type);
                    var turnedOffFrameX = TurnedOffFrameX[index];
                    MakeEdit(new FrameEdit(point.X, point.Y, turnedOffFrameX, tile.frameY));
                }
            }

            foreach (var worldEdit in Edits)
            {
                var worldEditPoint = new Point(worldEdit.X, worldEdit.Y);
                if (!tilesInSquare.Contains(worldEditPoint))
                {
                    worldEdit.Revert();
                }
            }
        }
    }
}