using System;
using System.IO;

namespace RailwayEssentialCore
{
    public static class Utils
    {
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
    }
}
