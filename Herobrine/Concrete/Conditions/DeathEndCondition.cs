using System.Collections.Generic;
using Herobrine.Abstract;
using Herobrine.Attributes;
using TerrariaApi.Server;

namespace Herobrine.Concrete.Conditions
{
    [HauntingItemDescription("death", "Stops the haunting on the players death. Arguments: None", "death")]
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

        public void Resume()
        {
        
        }

        public Dictionary<string, string> Save()
        {
            return null;
        }

        public void Load(Dictionary<string, string> state)
        {
            
        }
    }
}