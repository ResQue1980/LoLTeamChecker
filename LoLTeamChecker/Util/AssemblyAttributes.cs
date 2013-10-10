using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LoLTeamChecker.Util
{
    public static class AssemblyAttributes
    {

        public static T GetAttribute<T>() where T : Attribute
        {
            return typeof(AssemblyAttributes).Assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault(o => o is T) as T;
        }

        public static string FileVersion
        {
            get
            {
                var attr = GetAttribute<AssemblyFileVersionAttribute>();
                return (attr != null) ? attr.Version : null;
            }
        }
        public static string Configuration
        {
            get
            {
                var attr = GetAttribute<AssemblyConfigurationAttribute>();
                return (attr != null) ? attr.Configuration : null;
            }
        }
    }
}
