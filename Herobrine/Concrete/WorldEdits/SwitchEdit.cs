using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class SwitchEdit : IWorldEdit
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SwitchEdit(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Edit()
        {
            Wiring.hitSwitch(X, Y);
        }

        public void Revert()
        {
            Wiring.hitSwitch(X, Y);
        }
    }
}