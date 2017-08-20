using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace RailwayEssentialMdi.Registry
{
    public class Registry
    {
        private static string SubKeyAppName = "SOFTWARE\\RailwayEssential";
        private static string SubKeyRecentProjects = "Recent";
        private static int MaxRecent = 10;

        private readonly RegistryKey _appKey;

        public List<string> RecentProjects
        {
            get
            {
                if(_appKey == null)
                    return new List<string>();
                var v = _appKey.GetValue(SubKeyRecentProjects) as string;
                if(string.IsNullOrEmpty(v))
                    return new List<string>();
                var parts = v.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries);
                if (parts == null || parts.Length == 0)
                    return new List<string>();
                return parts.ToList();
            }
        }

        public void SetRecent(List<string> recentPaths)
        {
            if(recentPaths == null)
                recentPaths = new List<string>();

            var entries = new List<string>();
            if (recentPaths.Count > MaxRecent)
            {
                for (int i = 0; i < MaxRecent; ++i)
                    entries.Add(recentPaths[i]);
            }
            else
            {
                for(int i=0; i < recentPaths.Count; ++i)
                    entries.Add(recentPaths[i]);
            }

            if (_appKey != null)
                _appKey.SetValue(SubKeyRecentProjects, string.Join(";", entries));
        }

        private RegistryKey GetKey(string name)
        {
            RegistryKey myKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            RegistryKey key = myKey.OpenSubKey(name, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            if (key == null)
                key = myKey.CreateSubKey(name);
            return key;
        }

        public Registry()
        {
            _appKey = GetKey(SubKeyAppName);
        }       
    }
}
