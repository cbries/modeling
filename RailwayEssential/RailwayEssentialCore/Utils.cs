using System;
using System.IO;

namespace RailwayEssentialCore
{
    public static class Utils
    {
        public const string ThemeName = @"\Themes\SpDrS60used";
        public const string TrackplansEditor = @"\Trackplans\Webeditor";
        
        public static string ExpandRailwayEssential(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            var p = Environment.ExpandEnvironmentVariables("%RAILWAYESSENTIAL%");
            if (string.IsNullOrEmpty(p))
                return path;

            var pp = path.TrimStart('/', '\\');

            return Path.Combine(p, pp);
        }

        public static string GenerateUniqueName(this string fmt, string dirname=null)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (!string.IsNullOrEmpty(dirname))
                {
                    var name = Path.Combine(dirname, string.Format(fmt, i));
                    if (!File.Exists(name))
                        return name;
                }
                else
                {
                    var name = string.Format(fmt, i);
                    if (!File.Exists(name))
                        return name;
                }
            }

            return null;
        }

    }
}
