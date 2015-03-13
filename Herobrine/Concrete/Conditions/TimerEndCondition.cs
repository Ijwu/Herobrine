using System;
using System.Collections.Generic;
using System.Timers;
using Herobrine.Abstract;
using Herobrine.Attributes;

namespace Herobrine.Concrete.Conditions
{
    [HauntingItemDescription("timer", "Stops the haunting after a certain time. Arguments: [time in seconds]", "timer")]
    public class TimerEndCondition : IHauntingEndCondition
    {
        public IHaunting Haunting { get; set; }

        public Timer Timer { get; set; }

        public bool Elapsed { get; set; }

        public TimerEndCondition(IHaunting haunting)
        {
            Haunting = haunting;
        }

        public bool Update()
        {
            return Elapsed;
        }

        public bool ParseParameters(List<string> parameters)
        {
            if (parameters.Count == 0)
            {
                return false;
            }
            int time;
            if (!int.TryParse(parameters[0], out time))
            {
                return false;
            }
            Elapsed = false;
            Timer = new Timer(time*1000) {AutoReset = false};
            Timer.Elapsed += TimerOnElapsed;
            Timer.Start();
            return true;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Elapsed = true;
        }
    }
}