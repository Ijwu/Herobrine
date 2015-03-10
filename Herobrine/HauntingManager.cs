using System;
using System.Collections.Generic;
using Herobrine.Abstract;

namespace Herobrine
{
    public class HauntingManager
    {
        private Dictionary<IHaunting, IHauntingEndCondition> Hauntings { get; set; }

        public HauntingManager()
        {
            Hauntings = new Dictionary<IHaunting, IHauntingEndCondition>();
        }

        public void Update()
        {
            var toBeRemoved = new List<IHaunting>();
            foreach (var haunting in Hauntings)
            {
                if (haunting.Value.Update())
                {
                    toBeRemoved.Add(haunting.Key);
                    continue;
                }
                haunting.Key.Update();
            }
            foreach (var haunting in toBeRemoved)
            {
                Hauntings.Remove(haunting);
            }
        }

        public void AddHaunting(IHaunting haunting, IHauntingEndCondition endCondition)
        {
            if (haunting == null)
                throw new ArgumentNullException("haunting");

            if (endCondition == null)
                throw new ArgumentNullException("endCondition");

            Hauntings.Add(haunting, endCondition);
        }

        public void RemoveHaunting(IHaunting haunting)
        {
            if (Hauntings.ContainsKey(haunting))
            {
                Hauntings.Remove(haunting);
            }
        }
    }
}