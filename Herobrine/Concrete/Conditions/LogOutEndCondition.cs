﻿using System.Collections.Generic;
using Herobrine.Abstract;
using Herobrine.Attributes;

namespace Herobrine.Concrete.Conditions
{
    [HauntingItemDescription("logout", "Stops the haunting when the player logs out.", "logout")]
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