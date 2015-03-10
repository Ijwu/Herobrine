using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class FrameEdit : IWorldEdit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public short OldFrameX { get; private set; }
        public short OldFrameY { get; private set; }

        public FrameEdit(int x, int y, short framex, short framey)
        {
            X = x;
            Y = y;
            OldFrameX = Main.tile[x, y].frameX;
            OldFrameY = Main.tile[x, y].frameY;
            Main.tile[x, y].frameX = framex;
            Main.tile[x, y].frameY = framey;
        }

        public void Revert()
        {
            Main.tile[X, Y].frameX = OldFrameX;
            Main.tile[X, Y].frameY = OldFrameY;
        }   
    }
}