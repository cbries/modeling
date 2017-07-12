using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RailwayEssentialWeb
{
    public class WebTableGenerator
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public string ThemeDirectory { get; set; }

        public Random _randomNumberGenerator = new Random();

        public WebTableGenerator()
        {
            Rows = 20;
            Columns = 50;

            TileWidth = 32;
            TileHeight = 32;
        }

        private List<string> ThemeFiles
        {
            get { return Directory.GetFiles(ThemeDirectory, "*.svg", SearchOption.TopDirectoryOnly).ToList(); }
        }

        private string GetRandomSvg()
        {
            int index = _randomNumberGenerator.Next(0, ThemeFiles.Count - 1);
            return ThemeFiles[index];
        }

        private int _currentIndex = 0;
        private string GetNextSvg()
        {
            if (_currentIndex >= ThemeFiles.Count)
                _currentIndex = 0;
            var n = ThemeFiles[_currentIndex];
            ++_currentIndex;
            return n;
        }

        private string CreateBase()
        {
            string m = "<html><head>";
            m += "<link rel=\"stylesheet\" type=\"text/css\" href=\"theme.css\">";
            m += "<body>\r\n{{CONTENT}}\r\n</body></html>";
            return m;
        }
        
        public bool Generate(string targetDirectory)
        {
            string css = @".gridTrackPlan { border: 1px solid black; }
.cell { border: 1px solid lightgray; background-repeat:no-repeat; background-size: "+TileWidth+ @"px " + TileHeight + @"px; width: " + TileWidth + @"px; height: " + TileHeight + @"px; }
html, body { height: 100%;  }
html { display: table; margin: auto; }
body { display: table-cell; vertical-align: middle; }";

            File.WriteAllText(Path.Combine(targetDirectory, "theme.css"), css, Encoding.UTF8);

            string html = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"gridTrackPlan\">\r\n";

            for (int y = 0; y < Rows; ++y)
            {
                html += "  <tr>\r\n";

                for (int x = 0; x < Columns; ++x)
                {
                    var fname = GetNextSvg();
                    if (File.Exists(fname))
                    {
                        var targetPath = Path.Combine(targetDirectory, Path.GetFileName(fname));
                        File.Copy(fname, targetPath, true);
                    }

                    html += "    <td id=\"cell_" + x + "_" + y + "\" class=\"cell\" ";
                    html += "style=\"background-image:url("+ Path.GetFileName(fname) + ");\"";
                    html += "></td>\r\n";
                }

                html += "  </tr>\r\n";
            }

            html += "</table>";

            try
            {
                var b = CreateBase();
                b = b.Replace("{{CONTENT}}", html);

                File.WriteAllText(Path.Combine(targetDirectory, "trackplan.html"), b, Encoding.UTF8);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
