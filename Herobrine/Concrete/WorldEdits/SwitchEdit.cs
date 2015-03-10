using Herobrine.Abstract;
using Terraria;

namespace Herobrine.Concrete.WorldEdits
{
    public class SwitchEdit : IWorldEdit
    {
        private int _x;
        private int _y;
        private bool _oldState;

        public SwitchEdit(int x, int y)
        {
            _x = x;
            _y = y;
            if (Main.tile[x, y].inActive())
            {
                _oldState = false;
                Wiring.ReActive(x, y);
            }
            else
            {
                _oldState = true;
                Wiring.DeActive(x, y);
            }
        }

        public void Revert()
        {
            if (_oldState)
            {
                Wiring.ReActive(_x, _y);
            }
            else
            {
                Wiring.DeActive(_x, _y);
            }
        }
    }
}