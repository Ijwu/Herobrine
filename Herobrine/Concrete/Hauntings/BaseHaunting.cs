using System.Collections.Generic;
using Herobrine.Abstract;

namespace Herobrine.Concrete.Hauntings
{
    class BaseHaunting : IHaunting
    {
        public Victim Victim { get; private set; }
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

        public void CleanUp()
        {
            foreach (var worldEdit in Edits)
            {
                worldEdit.Revert();
            }
        }

        public void MakeEdit(IWorldEdit edit)
        {
            Edits.Add(edit);
        }
    }
}