using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RailwayEssentialCore;
using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public partial class WebGenerator : IWebGenerator
    {
        private Track _trackinfo;
        private string _targetFilepath;

        public int Rows { get; set; }
        public int Columns { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public string ThemeDirectory { get; set; }

        private readonly Random _randomNumberGenerator = new Random();

        public WebGenerator()
        {
            Rows = 15;
            Columns = 25;

            TileWidth = 24;
            TileHeight = 24;
        }

        private List<string> ThemeFiles => Directory.GetFiles(ThemeDirectory, "*.svg", SearchOption.TopDirectoryOnly).ToList();

        public string GetRandomSvg()
        {
            int index = _randomNumberGenerator.Next(0, ThemeFiles.Count - 1);
            return ThemeFiles[index];
        }

        private int _currentIndex;
        public string GetNextSvg()
        {
            if (_currentIndex >= ThemeFiles.Count)
                _currentIndex = 0;
            var n = ThemeFiles[_currentIndex];
            ++_currentIndex;
            return n;
        }

        public string GetSvg(string name)
        {
            return Path.Combine(ThemeDirectory, name + ".svg");
        }

        private string CreateSymbolSelection()
        {
            List<string> ignore = new List<string>()
            {
                "-off", "-on", "-on-occ", "-off-occ", "-on-route", "-off-route", "-occ", "-route",
                "traverser", "-bridge", "-bridge-res"
            };

            string m = "";
            foreach (var e in ThemeFiles)
            {
                if (e == null)
                    continue;

                var symbolName = Path.GetFileNameWithoutExtension(e);
                if (string.IsNullOrEmpty(symbolName))
                    continue;

                bool doIgnore = false;
                foreach (var ignoreE in ignore)
                {
                    if (symbolName.EndsWith(ignoreE, StringComparison.OrdinalIgnoreCase))
                    {
                        doIgnore = true;
                        break;
                    }
                }

                if (doIgnore)
                    continue;

                m += "<option value=\"" + symbolName + "\" data-image=\""+new Uri(e).AbsoluteUri+"\">" + symbolName + "</option>";
            }
            return m;
        }

        private string CreateBase()
        {
            var fname = @"Trackplans\Webeditor\template.html.keep".ExpandRailwayEssential();
            try
            {
                return File.ReadAllText(fname, Encoding.UTF8);
            }
            catch
            {
                return "";
            }
        }

        public void SetTrackInfo(Track info)
        {
            _trackinfo = info;
        }

        public bool Update()
        {
            if (string.IsNullOrEmpty(_targetFilepath))
                return false;

            return Generate(_targetFilepath);
        }

        public bool Generate(string targetFilepath)
        {
            StringBuilder oSb = new StringBuilder();

            oSb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"gridTrackPlan\">\r\n");

            for (int y = 0; y < Rows; ++y)
            {
                oSb.Append("  <tr>\r\n");

                for (int x = 0; x < Columns; ++x)
                {
                    //var fname = GetNextSvg();
                    var id = "cell_" + x + "_" + y;
                    var title = id;
                    var colspan = " colspan=\"1\" class=\"cell\" ";

                    var trackinfo = _trackinfo.Get(x + 1, y + 1);

                    if (trackinfo != null)
                    {
                        if(trackinfo.LengthX() == 4)
                            colspan = " colspan=\"4\" class=\"cell4x\" ";
                    }

                    // title=\"" + title + "\""
                    oSb.Append("<td id=\"" + id + "\"" + colspan);

                    var svgpath = "";
                    if (trackinfo != null)
                    {
                        string styleRotate = "";
                        switch (trackinfo.Orientation)
                        {
                                case Orientation.North:
                                    break;
                                case Orientation.East:
                                    styleRotate = "transform: rotate(90deg);";
                                    break;
                                case Orientation.South:
                                    styleRotate = "transform: rotate(180deg);";
                                    break;
                                case Orientation.West:
                                    styleRotate = "transform: rotate(-90deg);";
                                    break;
                        }

                        svgpath = GetSvg(trackinfo.IconName);
                        if (string.IsNullOrEmpty(svgpath))
                            oSb.Append("style=\"\"");
                        else
                            oSb.Append("style=\"background-repeat: no-repeat; background-position: center; background-image:url(" + new Uri(svgpath).AbsoluteUri + ");" + styleRotate + "\"");
                    }
                    else
                    {
                        oSb.Append("style=\"\"");
                    }

                    // ({x+1},{y+1})
                    var coordInfo =
                        $"<div style=\"color: black; font-weight: bold; text-align: center; font-size: 0.6em;  padding: 1px; vertical-align: middle;\"></div>";

                    oSb.Append(">" + coordInfo + "</td>\r\n");

                    if(trackinfo != null)
                        x += trackinfo.LengthX() - 1;
                }

                oSb.Append("</tr>\r\n");
            }

            oSb.Append("</table>");

            try
            {
                var b = CreateBase();
                    b = b.Replace("{{GLOBALJS}}", "var themeDirectory='"+new Uri(ThemeDirectory.Replace("\\", "/")).AbsoluteUri+"';");
                    b = b.Replace("{{TRACKTABLE}}", oSb.ToString());
                    b = b.Replace("{{TRACKSYMBOLS}}", CreateSymbolSelection());

                // remove old plans
                var files = Directory.GetFiles(Path.GetDirectoryName(targetFilepath), "*.html");
                foreach (var n in files)
                {
                    try
                    {
                        File.Delete(n);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                File.WriteAllText(targetFilepath, b, Encoding.UTF8);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
