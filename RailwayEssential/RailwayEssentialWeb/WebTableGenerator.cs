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

        public string ThemeDirectory { get; set; }

        public Random _randomNumberGenerator = new Random();

        public WebTableGenerator()
        {
            Rows = 20;
            Columns = 50;
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

        private string CreateBase()
        {
            string m = "<html><head>";
            m += "<link rel=\"stylesheet\" type=\"text/css\" href=\"theme.css\">";
            m += "<body>\r\n{{CONTENT}}\r\n</body></html>";
            return m;
        }
        
        public bool Generate(string targetFilename)
        {
            string html = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"gridTrackPlan\">\r\n";

            for (int y = 0; y < Rows; ++y)
            {
                html += "  <tr>\r\n";

                for (int x = 0; x < Columns; ++x)
                {
                    var fname = GetRandomSvg();
                    if (File.Exists(fname))
                    {
                        var targetDir = Path.GetDirectoryName(targetFilename);
                        var targetPath = Path.Combine(targetDir, Path.GetFileName(fname));
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

                File.WriteAllText(targetFilename, b, Encoding.UTF8);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
