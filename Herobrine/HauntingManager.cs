using System;
using System.Collections.Generic;
using Herobrine.Abstract;

namespace Herobrine
{
    public class HauntingManager
    {
        private Dictionary<int, List<IHaunting>> _hauntings = new Dictionary<int, List<IHaunting>>();

        public HauntingManager()
        {
            _hauntings = new Dictionary<int, List<IHaunting>>();
        }

        public void Update()
        {
            var toBeRemoved = new List<IHaunting>();
            foreach (var player in _hauntings)
            {
                foreach(var haunting in player.Value)
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
                    RemoveHaunting(player.Key, haunting);
                }
                toBeRemoved.Clear();
            }
        }

        public void AddHaunting(int userId, IHaunting haunting)
        {
            if (haunting == null)
                throw new ArgumentNullException("haunting");

            if (_hauntings.ContainsKey(userId))
            {
                _hauntings[userId].Add(haunting);
            }
            else
            {
                _hauntings.Add(userId, new List<IHaunting>());
                _hauntings[userId].Add(haunting);
            }
        }

        public void RemoveHaunting(int userId, IHaunting haunting)
        {
            List<IHaunting> hauntings;
            if (_hauntings.TryGetValue(userId, out hauntings))
            {
                haunting.CleanUp();
                hauntings.Remove(haunting);
            }
        }

        public List<IHaunting> GetHauntingsForPlayer(int userid)
        {
            List<IHaunting> ret;
            if (_hauntings.TryGetValue(userid, out ret))
            {
                return ret;
            }
            return null;
        }

        public void RemoveHauntingsForPlayer(int userId)
        {
            Herobrine.Debug("Removing hauntings for player {0}", userId);
            List<IHaunting> hauntings;
            if (_hauntings.TryGetValue(userId, out hauntings))
            {
                foreach (var haunting in hauntings)
                {
                    haunting.CleanUp();
                }
                hauntings.Clear();
            }
        }
    }
}