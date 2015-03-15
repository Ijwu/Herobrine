using System;
using System.Collections.Generic;
using Herobrine.Abstract;

namespace Herobrine
{
    public class HauntingManager
    {
        private List<IHaunting> Hauntings { get; set; }

        public HauntingManager()
        {
            Hauntings = new List<IHaunting>();
        }

        public void Update()
        {
            var toBeRemoved = new List<IHaunting>();
            foreach (var haunting in Hauntings)
            {
                if (haunting.EndCondition.Update())
                {
                    toBeRemoved.Add(haunting);
                    continue;
                }
                haunting.Update();
            }
            foreach (var haunting in toBeRemoved)
            {
                RemoveHaunting(haunting);
            }
        }

        public void AddHaunting(IHaunting haunting, IHauntingEndCondition endCondition)
        {
            if (haunting == null)
                throw new ArgumentNullException("haunting");

            if (endCondition == null)
                throw new ArgumentNullException("endCondition");

            haunting.EndCondition = endCondition;
            Hauntings.Add(haunting);
        }

        public void RemoveHaunting(IHaunting haunting)
        {
            if (Hauntings.Contains(haunting))
            {
                haunting.CleanUp();
                Hauntings.Remove(haunting);
            }
        }
    }
}