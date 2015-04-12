using System;
using System.Collections.Generic;
using System.Linq;
using Herobrine.Abstract;
using Herobrine.Attributes;
using Herobrine.Concrete.WorldEdits;
using Terraria;

namespace Herobrine.Concrete.Hauntings
{
    [HauntingItemDescription("LightsOut", "Turns off all lights around a player.", "lightsout")]
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
            //Get all tiles in an area around the player.
            var tilesInSquare = Victim.GetTilesInSquare(80);
            //Foreach tile in the area.
            foreach (var point in tilesInSquare)
            {
                //Check if it's already handled by the haunting.
                if (IsBeingEdited(point.X, point.Y)) continue;
                //If not already handled...
                var tile = Main.tile[point.X, point.Y];
                //If the tile is a light type that the haunting can handle.
                if (SwitchableLightIds.Contains(tile.type))
                {
                    //Get the index in the tile id array.
                    var index = Array.IndexOf(SwitchableLightIds, tile.type);
                    //Get the final framex which is the TurnedOffFrameX + current frame x.
                    //This ensures multi-tile compatibility.
                    short initialTurnedOffFrameX = TurnedOffFrameX[index];
                    short turnedOffFrameX = (short) (initialTurnedOffFrameX + tile.frameX);
                    //The intialTurnedOffFrameX is the minimum required frameX to be off.
                    //If the current frameX is less than it, that means the light is on and should be handled.
                    //Check for tile.active() == true to make compatible with special torch handling.
                    if (tile.frameX < initialTurnedOffFrameX && tile.active())
                    {
                        if (tile.type == 4)
                        {
                            //Special case handling for torches because Terraria is badly coded.
                            Tile newTile = new Tile(tile);
                            newTile.active(false);
                            MakeEdit(new TileEdit(point.X, point.Y, newTile));
                        }
                        else
                        {
                            MakeEdit(new FrameEdit(point.X, point.Y, turnedOffFrameX, tile.frameY));
                        }
                    }
                }
            }

            var toBeRemoved = new List<IWorldEdit>();
            //Checking if a tile is out of range now.
            foreach (var worldEdit in Edits)
            {
                var worldEditPoint = new Point(worldEdit.X, worldEdit.Y);
                //Check if the list of tiles around the player contains a point representing the IWorldEdit.
                if (!tilesInSquare.Contains(worldEditPoint))
                {
                    //If it's not in the list then it's too far away. Add it to the list to be removed.
                    toBeRemoved.Add(worldEdit);
                }
            }
            //Revert each tile in the list toBeRemoved.
            //We didn't do this in the foreach because it would alter the collection as it enumerated it. (big nono)
            foreach (var worldEdit in toBeRemoved)
            {
                RevertEdit(worldEdit);
            }
        }
    }
}