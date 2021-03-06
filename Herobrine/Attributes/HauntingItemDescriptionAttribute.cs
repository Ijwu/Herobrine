﻿using System;

namespace Herobrine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HauntingItemDescriptionAttribute : Attribute
    {
        public string Name { get; set; }
        public string HelpText { get; set; }
        public string Permission { get; set; }

        /// <summary>
        ///     Denotes an item for use within the herobrine plugin. May be a haunting or end condition.
        /// </summary>
        /// <param name="name">Name of the end condition.</param>
        /// <param name="helpText">Argument usage help text which is displayed when the condition is used incorrectly.</param>
        /// <param name="permission">Permission required to use this end condition.</param>
        public HauntingItemDescriptionAttribute(string name, string helpText, string permission)
        {
            Name = name;
            HelpText = helpText;
            Permission = permission;
        }
    }
}