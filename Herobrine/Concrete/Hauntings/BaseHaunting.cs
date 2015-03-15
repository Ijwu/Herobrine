using System.Collections.Generic;
using System.Linq;
using Herobrine.Abstract;

namespace Herobrine.Concrete.Hauntings
{
    public class BaseHaunting : IHaunting
    {
        public Victim Victim { get; private set; }

        public IHauntingEndCondition EndCondition { get; set; }

        public List<IWorldEdit> Edits { get; private set; }
        

        public BaseHaunting(Victim victim)
        {
            Victim = victim;
            Edits = new List<IWorldEdit>();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        public virtual void CleanUp()
        {
            Herobrine.Debug("Haunting cleanup start.");
            foreach (var worldEdit in Edits)
            {
                worldEdit.Revert();
                Victim.Player.SendTileSquare(worldEdit.X, worldEdit.Y);
            }
            Herobrine.Debug("Haunting cleanup end.");
        }

        public void MakeEdit(IWorldEdit edit)
        {
            edit.Edit();
            Edits.Add(edit);
            Victim.Player.SendTileSquare(edit.X, edit.Y);
        }

        public void RevertEdit(IWorldEdit edit)
        {
            if (Edits.Contains(edit))
            {
                edit.Revert();
                Victim.Player.SendTileSquare(edit.X, edit.Y);
                Edits.Remove(edit);
            }
        }

        public bool IsBeingEdited(int x, int y)
        {
            return Edits.Any(worldEdit => worldEdit.X == x && worldEdit.Y == y);
        }
    }
}