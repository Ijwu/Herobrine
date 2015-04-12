using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class FrameEdit : IWorldEdit
    {
        private readonly short _framex;
        private readonly short _framey;
        public int X { get; set; }
        public int Y { get; set; }
        public short OldFrameX { get; private set; }
        public short OldFrameY { get; private set; }

        public FrameEdit(int x, int y, short framex, short framey)
        {
            _framex = framex;
            _framey = framey;
            X = x;
            Y = y;
        }

        public void Edit()
        {
            OldFrameX = Main.tile[X, Y].frameX;
            OldFrameY = Main.tile[X, Y].frameY;
            Main.tile[X, Y].frameX = _framex;
            Main.tile[X, Y].frameY = _framey;
        }

        public void Revert()
        {
            Main.tile[X, Y].frameX = OldFrameX;
            Main.tile[X, Y].frameY = OldFrameY;
        }
    }
}