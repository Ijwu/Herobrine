using System;
using System.Collections.Generic;
using System.IO;
using Herobrine.Abstract;
using Newtonsoft.Json;
using TShockAPI;

namespace Herobrine.Concrete.Repositories
{
    public class JsonHauntingRepository : IHauntingRepository
    {
        private Dictionary<int, List<IHaunting>> _hauntings = new Dictionary<int, List<IHaunting>>();
        private string _dbPath;
        public JsonHauntingRepository()
        {
            _dbPath = Path.Combine(TShock.SavePath, "herobrine");
            
            foreach (var file in Directory.EnumerateFiles(_dbPath))
            {
                try
                {
                    var fs = new FileStream(Path.Combine(_dbPath, file), FileMode.Open, FileAccess.Read);
                    var stream = new StreamReader(fs);
                    var data = JsonConvert.DeserializeObject<JsonPlayer>(stream.ReadToEnd());
                    _hauntings[data.Id] = InstantiateHauntingsFromJson(data.Id, data.Hauntings);
                }
                catch (JsonSerializationException)
                {
                    Herobrine.Debug("JSON deserialization exception occured in loading hauntings from file {0}.", file);
                }
            }
        }

        /// <summary>
        /// Gets the player's current hauntings.
        /// </summary>
        /// <param name="userId">Player to load hauntings for.</param>
        /// <returns>List of IHaunting or null if the player did not exist in the database.</returns>
        public List<IHaunting> GetHauntingsForPlayer(int userId)
        {
            List<IHaunting> hauntings;
            if (_hauntings.TryGetValue(userId, out hauntings))
            {
                return hauntings;
            }
            return null;
        }

        public void SavePlayerHauntings(int userId, List<IHaunting> hauntings)
        {
            if (_hauntings.ContainsKey(userId))
            {
                _hauntings[userId] = hauntings;
            }
            else
            {
                _hauntings.Add(userId, hauntings);
            }
            SavePlayerHauntingsToDisk(userId);
        }

        private void SavePlayerHauntingsToDisk(int userId)
        {
            try
            {
                var fs = new FileStream(Path.Combine(_dbPath, string.Format("{0}.{1}", userId, ".json")), FileMode.OpenOrCreate, FileAccess.Write);
                var str = JsonConvert.SerializeObject(_hauntings[userId], Formatting.Indented);
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(str);
                }
            }
            catch (JsonSerializationException)
            {
                Herobrine.Debug("JSON serialization exception occured in saving hauntings for player {0}.", userId);
            }
        }

        private IHaunting InstantiateHauntingFromJson(int user, JsonHaunting jsonHaunting)
        {
            var hauntingType = Herobrine.HauntingTypes.GetHauntingTypeFromName(jsonHaunting.HauntingName);
            var endType = Herobrine.HauntingTypes.GetEndConditionTypeFromName(jsonHaunting.EndConditionName);
            IHaunting haunting = null;
            IHauntingEndCondition endCond = null;
            try
            {
                haunting = (IHaunting)Activator.CreateInstance(hauntingType, new Victim(user));
                endCond = (IHauntingEndCondition)Activator.CreateInstance(endType, haunting);
                endCond.Load(jsonHaunting.EndConditionState);
                haunting.EndCondition = endCond;
            }
            catch (Exception e)
            {
                Herobrine.Debug("Haunting failed to load. Exception message follows.");  
                Herobrine.Debug(e.ToString());
            }
            return haunting;
        }

        private List<IHaunting> InstantiateHauntingsFromJson(int user, List<JsonHaunting> hauntings)
        {
            var ret = new List<IHaunting>();
            foreach (var jsonHaunting in hauntings)
            {
                ret.Add(InstantiateHauntingFromJson(user, jsonHaunting));
            }
            return ret;
        }
    }

    public class JsonPlayer
    {
        public int Id { get; set; }
        public List<JsonHaunting> Hauntings { get; set; } 
    }

    public class JsonHaunting
    {
        public string HauntingName { get; set; }
        public string EndConditionName { get; set; }
        public Dictionary<string, string> EndConditionState { get; set; }
    }
}