using System;

namespace Herobrine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HauntingEndConditionAttribute : Attribute
    {
        public string Name { get; set; }
        public string HelpText { get; set; }

        /// <summary>
        /// Denotes a haunting end condition.
        /// </summary>
        /// <param name="name">Name of the end condition.</param>
        /// <param name="helpText">Argument usage help text which is displayed when the condition is used incorrectly.</param>
        public HauntingEndConditionAttribute(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
        }
    }
}