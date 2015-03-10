using System;

namespace Herobrine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HauntingAttribute : Attribute
    {
        public string Name { get; set; }
        public string Permission { get; set; }

        public HauntingAttribute(string name, string permission)
        {
            Name = name;
            Permission = permission;
        }
    }
}