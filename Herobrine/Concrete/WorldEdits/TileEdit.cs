using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class TileEdit : IWorldEdit
    {
        private int _x;
        private int _y;
        public Tile OldTile { get; private set; }
        
        public TileEdit(int x, int y, Tile newTile)
        {
            _x = x;
            _y = y;
            OldTile = new Tile(Main.tile[x, y]);
        }

        public void Revert()
        {
            Main.tile[_x,_y].CopyFrom(OldTile);
        }
    }
}