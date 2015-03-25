using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Herobrine.Abstract;
using Newtonsoft.Json;
using TShockAPI;

namespace Herobrine.Concrete.Repositories
{
    public class JsonHauntingRepository : IHauntingRepository
    {
        /// <summary>
        /// Internal cache of hauntings for each player.
        /// Used for saving and resuming.
        /// </summary>
        private Dictionary<int, JsonPlayer> _hauntings = new Dictionary<int, JsonPlayer>();
        private string _dbPath;

        public JsonHauntingRepository()
        {
            _dbPath = Path.Combine(TShock.SavePath, "herobrine");

            if (!Directory.Exists(_dbPath))
            {
                Directory.CreateDirectory(_dbPath);
                return;
            }

            foreach (var file in Directory.EnumerateFiles(_dbPath))
            {
                try
                {
                    var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    var stream = new StreamReader(fs);
                    var data = JsonConvert.DeserializeObject<JsonPlayer>(stream.ReadToEnd());
                    _hauntings[data.Id] = data;
                }
                catch (JsonSerializationException)
                {
                    Herobrine.Debug("JSON deserialization exception occured in loading hauntings from file {0}.",
                        file);
                }
            }
        }

        /// <summary>
        /// Gets the player's current hauntings.
        /// </summary>
        /// <param name="userId">Player to load hauntings for.</param>
        /// <returns>List of IHaunting or null if the player did not exist in the database.</returns>
        public List<IHaunting> GetSuspendedHauntingsForPlayer(int userId)
        {
            JsonPlayer player;
            if (_hauntings.TryGetValue(userId, out player))
            {
                return InstantiateHauntingsListFromJson(userId, player.Hauntings);
            }
            return null;
        }

        public void SavePlayerHauntings(int userId, List<IHaunting> hauntings)
        {
            JsonPlayer player;
            if (_hauntings.TryGetValue(userId, out player))
            {
                player.Hauntings = GetJsonHauntingsFromList(hauntings);
            }
            else
            {
                player = new JsonPlayer();
                player.Id = userId;
                player.Hauntings = GetJsonHauntingsFromList(hauntings);
                _hauntings.Add(userId, player);
            }
            SavePlayerHauntingsToDisk(userId);
        }

        private void SavePlayerHauntingsToDisk(int userId)
        {
            try
            {
                var fs = new FileStream(Path.Combine(_dbPath, string.Format("{0}.{1}", userId, "json")), FileMode.OpenOrCreate, FileAccess.Write);
                var str = JsonConvert.SerializeObject(_hauntings[userId], Formatting.Indented);
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(str);
                }
                fs.Flush();
                fs.Dispose();
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
            try
            {
                haunting = (IHaunting)Activator.CreateInstance(hauntingType, new Victim(user));
                var endCond = (IHauntingEndCondition)Activator.CreateInstance(endType, haunting);
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

        private List<JsonHaunting> GetJsonHauntingsFromList(List<IHaunting> hauntings)
        {
            var ret = new List<JsonHaunting>();
            foreach (var haunting in hauntings)
            {
                var jsonHaunting = new JsonHaunting();
                jsonHaunting.HauntingName = Herobrine.HauntingTypes.GetHauntingTypeNameFromType(haunting.GetType());
                jsonHaunting.EndConditionName = Herobrine.HauntingTypes.GetHauntingTypeNameFromType(haunting.EndCondition.GetType());
                jsonHaunting.EndConditionState = haunting.EndCondition.Save();
                ret.Add(jsonHaunting);
            }
            return ret;
        }

        private List<IHaunting> InstantiateHauntingsListFromJson(int user, List<JsonHaunting> hauntings)
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