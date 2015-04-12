using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Herobrine.Attributes;
using TShockAPI;

namespace Herobrine
{
    public class HauntingTypesContainer
    {
        public HauntingTypesContainer()
        {
            HauntingTypes = new List<Type>();
            EndConditionTypes = new List<Type>();
        }

        public List<Type> HauntingTypes { get; private set; }
        public List<Type> EndConditionTypes { get; private set; }

        public void DisplayHauntingsHelp(TSPlayer target, int i)
        {
            PaginationTools.SendPage(target, i, PaginationTools.BuildLinesFromTerms(GetHauntingNamesList(),
                o =>
                    string.Format("{0} - Permission: {1} - {2}", o, GetHauntingItemPermission((string) o),
                        GetHauntingItemHelpText((string) o))), new PaginationTools.Settings()
                        {
                            FooterFormat = "Type /haunt -h {0} for more."
                        });
        }

        public void DisplayConditionsHelp(TSPlayer target, int i)
        {
            PaginationTools.SendPage(target, i, PaginationTools.BuildLinesFromTerms(GetEndConditionNamesList(),
                o =>
                    string.Format("{0} - Permission: {1} - {2}", o, GetHauntingItemPermission((string) o),
                        GetHauntingItemHelpText((string) o))), new PaginationTools.Settings()
                        {
                            FooterFormat = "Type /haunt -c {0} for more."
                        });
        }

        public string GetHauntingItemHelpText(string name)
        {
            string ret = null;
            foreach (var conditionType in EndConditionTypes.Concat(HauntingTypes))
            {
                ForeachAttribute(conditionType, delegate(HauntingItemDescriptionAttribute haunt)
                {
                    if (haunt.Name.ToLower() == name.ToLower())
                    {
                        ret = haunt.HelpText;
                    }
                });
            }
            return ret;
        }

        public string GetHauntingItemPermission(string name)
        {
            string ret = null;
            foreach (var hauntingType in HauntingTypes.Concat(EndConditionTypes))
            {
                ForeachAttribute(hauntingType, delegate(HauntingItemDescriptionAttribute haunt)
                {
                    if (haunt.Name.ToLower() == name.ToLower())
                    {
                        ret = haunt.Permission;
                    }
                });
            }
            return ret;
        }

        public List<string> GetHauntingNamesList()
        {
            var ret = new List<string>();
            foreach (var hauntingType in HauntingTypes)
            {
                ForeachAttribute(hauntingType, delegate(HauntingItemDescriptionAttribute haunt) { ret.Add(haunt.Name); });
            }
            return ret;
        }

        public Type GetHauntingTypeFromName(string name)
        {
            Type ret = null;
            foreach (var hauntingType in HauntingTypes)
            {
                ForeachAttribute(hauntingType, delegate(HauntingItemDescriptionAttribute attribute)
                {
                    if (attribute.Name.ToLower() == name.ToLower())
                    {
                        // ReSharper disable once AccessToForEachVariableInClosure
                        ret = hauntingType;
                    }
                });
            }
            return ret;
        }

        public Type GetEndConditionTypeFromName(string name)
        {
            Type ret = null;
            foreach (var endConditionType in EndConditionTypes)
            {
                ForeachAttribute(endConditionType,
                    delegate(HauntingItemDescriptionAttribute cond)
                    {
                        if (cond.Name.ToLower() == name.ToLower())
                            // ReSharper disable once AccessToForEachVariableInClosure
                            ret = endConditionType;
                    });
            }
            return ret;
        }

        public List<string> GetEndConditionNamesList()
        {
            var ret = new List<string>();
            foreach (var endConditionType in EndConditionTypes)
            {
                ForeachAttribute(endConditionType,
                    delegate(HauntingItemDescriptionAttribute cond) { ret.Add(cond.Name); });
            }
            return ret;
        }

        public string GetHauntingTypeNameFromType(Type endConditionType)
        {
            var attrs = endConditionType.GetCustomAttributes(typeof (HauntingItemDescriptionAttribute));
            return ((HauntingItemDescriptionAttribute) attrs.First()).Name;
        }

        public void ForeachAttribute<T>(Type targetType, Action<T> func) where T : Attribute
        {
            var attributes = targetType.GetCustomAttributes(typeof (T), true);
            foreach (var attribute in attributes)
            {
                var attr = attribute as T;
                func(attr);
            }
        }
    }
}