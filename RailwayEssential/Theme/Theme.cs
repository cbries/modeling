using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Theme
{
    public class Theme
    {

        private List<string> _categoryNames = null;

        public List<string> CategoryNames
        {
            get
            {
                if (Categories == null || Categories.Count <= 0)
                    return null;

                if (_categoryNames == null)
                {
                    _categoryNames = new List<string>();

                    foreach (var e in Categories)
                    {
                        if (e == null)
                            continue;
                        if (string.IsNullOrEmpty(e.Name))
                            continue;

                        _categoryNames.Add(e.Name);
                    }
                }

                return _categoryNames;
            }
        }

        // [key:=name, value:=physical name]
        public Dictionary<string, string> GetDefaultForCategory(string catname)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(catname))
                return items;

            foreach (var e in Categories)
            {
                if (e == null)
                    continue;

                if (e.Name.Equals(catname, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var ee in e.Objects)
                    {
                        if (ee == null)
                            continue;

                        var name = ee.Name;
                        var physicalName = ee.Off.Default;

                        if (items.ContainsKey(name))
                            items[name] = physicalName;
                        else
                            items.Add(name, physicalName);
                    }
                }
            }

            return items;
        }

        public List<ThemeCategory> Categories { get; set; }

        public ThemeItem Get(int themeItemId)
        {
            if (themeItemId <= 0)
                return null;

            foreach (var e in Categories)
            {
                if (e == null)
                    continue;
                foreach (var ee in e.Objects)
                {
                    if (ee == null)
                        continue;

                    if (ee.UniqueIdentifier == themeItemId)
                        return ee;
                }
            }

            return null;
        }

        public Theme()
        {
            Categories = new List<ThemeCategory>();
        }

        public bool Load(string themeJsonFilePath)
        {
            if (string.IsNullOrEmpty(themeJsonFilePath))
                return false;
            if (!File.Exists(themeJsonFilePath))
                return false;

            try
            {
                string cnt = File.ReadAllText(themeJsonFilePath, Encoding.UTF8);
                if (string.IsNullOrEmpty(cnt))
                    return false;

                JArray ar = JArray.Parse(cnt);
                foreach (var e in ar)
                {
                    if (e == null)
                        continue;

                    var cat = new ThemeCategory();
                    if (cat.Parse(e))
                        Categories.Add(cat);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("<Theme> " + ex.Message);
                return false;
            }

            return true;
        }

    }
}
