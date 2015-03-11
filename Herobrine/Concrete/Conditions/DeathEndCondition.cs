using System.Collections.Generic;
using Herobrine.Abstract;
using TerrariaApi.Server;

namespace Herobrine.Concrete.Conditions
{
    public class DeathEndCondition : IHauntingEndCondition
    {
        public IHaunting Haunting { get; set; }

        public DeathEndCondition(IHaunting haunting)
        {
            Haunting = haunting;
        }

        public bool Update()
        {
            return Haunting.Victim.Player.Dead;
        }

        public bool ParseParameters(List<string> parameters)
        {
            return true;
        }
    }
}