﻿using System.Collections.Generic;
using System.Linq;
using Terraria;
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

        public Victim(int userId)
        {
            var user = TShock.Users.GetUserByID(userId);
            var player = TShock.Players.Single(x =>
            {
                if (x == null)
                {
                    return false;
                }
                return x.Name == user.Name;
            });
            Player = player;
        }

        /// <summary>
        ///     Returns a list of points containing all tiles in a square of the given length, centered on the player.
        /// </summary>
        public List<Point> GetTilesInSquare(int length)
        {
            var ret = new List<Point>();
            var sideLength = length/2;
            var playerX = Player.TileX;
            var playerY = Player.TileY;
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    ret.Add(new Point(playerX - i, playerY - j));
                    ret.Add(new Point(playerX + i, playerY - j));
                    ret.Add(new Point(playerX - i, playerY + j));
                    ret.Add(new Point(playerX + i, playerY + j));
                }
            }
            ret = ret.Where(x => x.X > 0 && x.Y > 0 && x.X < Main.maxTilesX && x.Y < Main.maxTilesY).ToList();
            return ret;
        }
    }
}