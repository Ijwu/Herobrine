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

        public TimerPlus Timer { get; set; }

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
            Timer = new TimerPlus(time*1000) {AutoReset = false};
            Timer.Elapsed += TimerOnElapsed;
            Timer.Start();
            return true;
        }

        public Dictionary<string, string> Save()
        {
            return new Dictionary<string, string>()
            {
                {"TimeLeft", Timer.TimeLeft.ToString()}
            };
        }

        public void Load(Dictionary<string, string> state)
        {
            string timeLeftString;
            if (state.TryGetValue("TimeLeft", out timeLeftString))
            {
                double timeLeft;
                if (double.TryParse(timeLeftString, out timeLeft))
                {
                    Timer = new TimerPlus(timeLeft);
                    Timer.Elapsed += TimerOnElapsed;
                    Timer.Start();
                }
                else
                {
                    throw new FormatException("TimeLeft is formatted incorrectly.");
                }
            }
            else
            {
                throw new FormatException("State dictionary provided is formatted incorrectly.");
            }
        }
        
        private void TimerOnElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            Elapsed = true;
        }
    }
}