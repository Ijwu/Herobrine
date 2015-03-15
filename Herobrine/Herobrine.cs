using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Herobrine.Abstract;
using Herobrine.Attributes;
using Herobrine.Concrete.Conditions;
using Herobrine.Concrete.Hauntings;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Utils = TShockAPI.Utils;

namespace Herobrine
{
    [ApiVersion(1,17)]
    public class Herobrine : TerrariaPlugin
    {
        public override string Name
        {
            get { return "Herobrine"; }
        }

        public override Version Version
        {
            get { return new Version(1,0,0); }
        }

        public override string Author
        {
            get { return "Iwju"; }
        }

        public override string Description
        {
            get { return "Allows you to haunt a player. Spoopy."; }
        }

        public HauntingManager Manager { get; set; }
        private Timer UpdateTimer { get; set; }

        internal static bool Debugging { get; set; }

        public static readonly HauntingTypesContainer HauntingTypes;

        static Herobrine()
        {
            HauntingTypes = new HauntingTypesContainer();
        }

        public Herobrine(Main game) : base(game)
        {
#if DEBUG
            Debugging = true;
#else
            Debugging = false;
#endif
            Manager = new HauntingManager();
            UpdateTimer = new Timer(OnUpdate, null, Timeout.Infinite, 1000/60);
        }

        internal static void Debug(string text, params object[] parms)
        {
            if (Debugging)
                TShock.Log.ConsoleInfo(text, parms);
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("herobrine.haunt", HauntPlayer, "haunt"));

            HauntingTypes.HauntingTypes.Add(typeof(LightsOutHaunting));
            HauntingTypes.EndConditionTypes.Add(typeof(TimerEndCondition));
            HauntingTypes.EndConditionTypes.Add(typeof(DeathEndCondition));
            HauntingTypes.EndConditionTypes.Add(typeof(LogOutEndCondition));

            UpdateTimer.Change(0, 1000/60);
        }

        private void OnUpdate(object state)
        {
            Manager.Update();
        }

        private void HauntPlayer(CommandArgs args)
        {
            TSPlayer target = null;
            //Check for params.
            if (args.Parameters.Count > 0)
            {
                //Get first param.
                var plr = args.Parameters[0];

                //Check for help flags.
                switch (plr)
                {
                    case "-h":
                    {
                        int page = 1;
                        if (args.Parameters.Count > 1)
                        {
                            if (int.TryParse(args.Parameters[1], out page))
                            {
                                HauntingTypes.DisplayHauntingsHelp(args.Player, page);
                                return;
                            }
                        }
                        HauntingTypes.DisplayHauntingsHelp(args.Player, page);
                        return;
                    }
                    case "-c":
                    {
                        int page = 1;
                        if (args.Parameters.Count > 1)
                        {
                            if (int.TryParse(args.Parameters[1], out page))
                            {
                                HauntingTypes.DisplayConditionsHelp(args.Player, page);
                                return;
                            }
                        }
                        HauntingTypes.DisplayConditionsHelp(args.Player, page);
                        return;
                    }
                }

                //If no help flags, we're here now. Read target player name and try to find them.
                var players = Utils.Instance.FindPlayer(plr);
                if (players.Count != 1)
                {
                    if (players.Count > 1)
                    {
                        args.Player.SendErrorMessage("Found multiple players {0}.", string.Join(", ", players));
                        return;
                    }
                    args.Player.SendErrorMessage("No player found by the name {0}.", plr);
                    return;
                }
                target = players.Single();
            }
            else
            {
                args.Player.SendErrorMessage("Command format: /haunt <Player> <HauntingType> <EndCondition> [ConditionArgs]");
                return;
            }

            //If target successfully found, we're here. Read haunting name and try to find it.
            IHaunting haunting;
            if (args.Parameters.Count > 1)
            {
                //Get haunting name.
                var hauntName = args.Parameters[1];
                //Try to get haunting type. Returns null if not found.
                var type = HauntingTypes.GetHauntingTypeFromName(hauntName);
                //If null, there's an error. Send them the haunts help page as a corrective measure.
                if (type == null)
                {
                    args.Player.SendInfoMessage("Incorrect haunting type '{0}'. Correct types are listed below.", hauntName);
                    HauntingTypes.DisplayHauntingsHelp(target, 1);
                    return;
                }
                //If we found the haunt type successfully we're here now. Check if the player has the permission to use the haunt.
                var hauntingPermission = HauntingTypes.GetHauntingItemPermission(hauntName);
                hauntingPermission = string.Format("herobrine.haunting.{0}", hauntingPermission);
                if (!args.Player.Group.HasPermission(hauntingPermission) || !args.Player.Group.HasPermission("herobrine.haunting.*"))
                {
                    //If the player doesn't have the permission, let them know.
                    args.Player.SendErrorMessage("You do not have permission to use the haunt '{0}'.", hauntName);
                    return;
                }

                //Now try to instantiate it.
                try
                {
                    haunting = (IHaunting) Activator.CreateInstance(type, new Victim(target));
                }
                catch(Exception e)
                {
                    //Gracefully log errors and exit should there be an issue.
                    args.Player.SendErrorMessage(
                        "An error has occured in the command. Please notify your server administrator. The exception is logged.");
                    TShock.Log.Error("Exception occured in Herobrine. Command was: {0}", args.Message);
                    TShock.Log.Error(e.ToString());
                    return;
                }
            }
            else
            {
                args.Player.SendErrorMessage("Command format: /haunt <Player> <HauntingType> <EndCondition> [ConditionArgs]");
                return;
            }

            //If we're here then we've successfully found the haunting type and have instantiated it.
            IHauntingEndCondition endCondition = null;
            if (args.Parameters.Count > 2)
            {
                //Get end condition name.
                var conditionName = args.Parameters[2];
                //Get end condition type using its name.
                var type = HauntingTypes.GetEndConditionTypeFromName(conditionName);

                //If the target was not found, show them the conditions help page.
                if (type == null)
                {
                    args.Player.SendInfoMessage("Incorrect condition type '{0}'. Correct types are listed below.", conditionName);
                    HauntingTypes.DisplayConditionsHelp(target, 1);
                    return;
                }

                //Check if the player has the permission to use the condition.
                var conditionPermission = HauntingTypes.GetHauntingItemPermission(conditionName);
                conditionPermission = string.Format("herobrine.condition.{0}", conditionPermission);
                if (!args.Player.Group.HasPermission(conditionPermission) || !args.Player.Group.HasPermission("herobrine.condition.*"))
                {
                    //If the player doesn't have the permission, let them know.
                    args.Player.SendErrorMessage("You do not have permission to use the end condition '{0}'.", conditionName);
                    return;
                }

                //If we made it over here then we have a proper end condition.
                try
                {
                    //Instantiate the condition.
                    endCondition = (IHauntingEndCondition) Activator.CreateInstance(type, haunting);
                }
                catch (Exception e)
                {
                    //Gracefully log errors and exit should there be an issue.
                    args.Player.SendErrorMessage(
                        "An error has occured in the command. Please notify your server administrator. The exception is logged.");
                    TShock.Log.Error("Exception occured in Herobrine. Command was: {0}", args.Message);
                    TShock.Log.Error(e.ToString());
                    return;
                }
            }
            else
            {
                args.Player.SendErrorMessage("Command format: /haunt <Player> <HauntingType> <EndCondition> [ConditionArgs]");
                return;
            }

            //If we've made it this far then we have both a good haunt type and good condition type.
            //Time to get on with the haunting.

            //Get the rest of the params as condition params.
            var endConditionArgs = args.Parameters.Skip(3).ToList();

            //Send the condition params to the condition. It decides if they are valid.
            if (endCondition.ParseParameters(endConditionArgs))
            {
                //One last try-catch, for good measure.
                try
                {
                    Manager.AddHaunting(haunting, endCondition);
                    args.Player.SendSuccessMessage("You have haunted {0}.", target.Name);
                }
                catch (Exception e)
                {
                    //Gracefully log errors and exit should there be an issue.
                    args.Player.SendErrorMessage(
                        "An error has occured in the command. Please notify your server administrator. The exception is logged.");
                    TShock.Log.Error("Exception occured in Herobrine. Command was: {0}", args.Message);
                    TShock.Log.Error(e.ToString());
                    return;
                }
            }
            else
            {
                args.Player.SendErrorMessage("Invalid arguments for the chosen condition.");
                HauntingTypes.ForeachAttribute<HauntingItemDescriptionAttribute>(endCondition.GetType(),
                    attribute => args.Player.SendErrorMessage(attribute.HelpText));
                return;
            }
        }
    }
}
