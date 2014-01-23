using System;

namespace University.DAL
{
        [AttributeUsage(AttributeTargets.Property)]
        public class IncludeAttribute : Attribute
        {
            public string TargetProperty { get; set; }
        }
}
