using System;

namespace Herobrine
{
    /// <summary>
    /// Copied from http://stackoverflow.com/users/637968/mike on the question http://stackoverflow.com/questions/2278525/system-timers-timer-how-to-get-the-time-remaining-until-elapse.
    /// Thanks. Mike.
    /// </summary>
    public class TimerPlus : System.Timers.Timer
    {
        private DateTime _dueTime;

        public TimerPlus()
        {
            Elapsed += ElapsedAction;
        }

        public TimerPlus(double interval) : base(interval)
        {
            
        }

        protected new void Dispose()
        {
            Elapsed -= ElapsedAction;
            base.Dispose();
        }

        public double TimeLeft
        {
            get
            {
                return (_dueTime - DateTime.Now).TotalMilliseconds;
            }
        }

        public new void Start()
        {
            this._dueTime = DateTime.Now.AddMilliseconds(Interval);
            base.Start();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AutoReset)
            {
                _dueTime = DateTime.Now.AddMilliseconds(Interval);
            }
        }
    }
}