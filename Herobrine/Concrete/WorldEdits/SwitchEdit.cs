using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class SwitchEdit : IWorldEdit
    {
        private int _x;
        private int _y;

        public SwitchEdit(int x, int y)
        {
            _x = x;
            _y = y;
            Wiring.hitSwitch(x, y);
        }

        public void Revert()
        {
            Wiring.hitSwitch(_x, _y);
        }
    }
}