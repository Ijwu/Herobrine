using System.Collections.Generic;
using Herobrine.Abstract;

namespace Herobrine.Concrete.Conditions
{
    public class LogOutEndCondition : IHauntingEndCondition
    {
        public IHaunting Haunting { get; set; }

        public LogOutEndCondition(IHaunting haunting)
        {
            Haunting = haunting;
        }

        public bool Update()
        {
            return !Haunting.Victim.Player.IsLoggedIn;
        }

        public bool ParseParameters(List<string> parameters)
        {
            return true;
        }
    }
}