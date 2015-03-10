using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class TileEdit : IWorldEdit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tile OldTile { get; private set; }
        
        public TileEdit(int x, int y, Tile newTile)
        {
            X = x;
            Y = y;
            OldTile = new Tile(Main.tile[x, y]);
        }

        public void Revert()
        {
            Main.tile[X,Y].CopyFrom(OldTile);
        }
    }
}