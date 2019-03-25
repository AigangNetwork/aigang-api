using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aigang.Platform.Utils
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration { get; set; }

        public static string GetConnectionString(string key)
        {
            return Configuration.GetConnectionString(key);
        }

        public static string GetString(string key)
        {
            return Configuration[key];
        }

        public static int GetInt(string key)
        {
            return Convert.ToInt32(Configuration[key]);
        }

        public static double GetDouble(string key)
        {
            return Convert.ToDouble(Configuration[key]);
        }
        
        public static bool GetBool(string key)
        {
            return Convert.ToBoolean(Configuration[key]);
        }

        public static List<string> GetStringList(string key)
        {
            return Configuration.GetSection(key).AsEnumerable().Select(item => item.Value).ToList();
        }
    }
}