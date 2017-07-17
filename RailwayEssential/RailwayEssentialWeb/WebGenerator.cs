using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;

namespace RailwayEssentialWeb
{
    public partial class WebGenerator : IWebGenerator
    {
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

        private Dictionary<string, List<string>> _symbols = new Dictionary<string, List<string>>();

        private string CreateSymbolSelection()
        {
            JArray arOrder = null;

            var orderJson = @"\Themes\SpDr560.json".ExpandRailwayEssential();
            if (File.Exists(orderJson))
            {
                string cnt = File.ReadAllText(orderJson, Encoding.UTF8);
                if (!string.IsNullOrEmpty(cnt))
                    arOrder = JArray.Parse(cnt);
            }

            foreach (var arLine in arOrder)
            {
                if (arLine == null)
                    continue;

                var o = arLine as JObject;
                if (o == null)
                    continue;

                if (o["category"] != null)
                {
                    string categoryName = o["category"].ToString();
                    if (string.IsNullOrEmpty(categoryName))
                        continue;

                    if (o["items"] != null)
                    {
                        JArray arr = o["items"] as JArray;
                        if (arr == null)
                            continue;

                        foreach(var arSymbol in arr)
                        {
                            if(arSymbol != null)
                            {
                                if (_symbols.ContainsKey(categoryName))
                                    _symbols[categoryName].Add(arSymbol.ToString());
                                else
                                    _symbols.Add(categoryName, new List<string>() {arSymbol.ToString()});
                            }
                        }
                    }
                }
            }

            // TODO TODO

            string m = "";
            foreach (var symbol in arOrder)
            {                
                foreach (var e in ThemeFiles)
                {
                    if (e.EndsWith(symbol.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var symbolName = Path.GetFileNameWithoutExtension(e);
                        if (string.IsNullOrEmpty(symbolName))
                            continue;

                        m += "<option value=\"" + symbolName + "\" data-image=\"" + new Uri(e).AbsoluteUri + "\">" + symbolName + "</option>";

                        break;
                    }
                }
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

        public bool Update()
        {
            if (string.IsNullOrEmpty(_targetFilepath))
                return false;

            return Generate(_targetFilepath);
        }

        public bool Generate(string targetFilepath)
        {
            StringBuilder oSb = new StringBuilder();

            oSb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border: 0;\">");
            for (int y = 0; y < Rows; ++y)
            {
                oSb.Append("<tr class=\"row\">");
                for (int x = 0; x < Columns; ++x)
                {
                    oSb.Append("<td class=\"cell\"><div class=\"overflow\"></div></td></td>");
                }
                oSb.Append("</tr>\r\n");
            }
            oSb.Append("</table>");

            string css = string.Format("table {{width:{0}px; height:{1}px;}}", Columns * 32, Rows * 32);

            try
            {
                var b = CreateBase();
                    b = b.Replace("{{GLOBALJS}}", "var themeDirectory='"+new Uri(ThemeDirectory.Replace("\\", "/")).AbsoluteUri+"';");
                    b = b.Replace("{{GLOBALCSS}}", css);
                    b = b.Replace("{{TRACKTABLE}}", oSb.ToString());
                    b = b.Replace("{{TRACKSYMBOLS}}", CreateSymbolSelection());

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
