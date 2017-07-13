using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RailwayEssentialWeb
{
    public partial class WebGenerator : IWebGenerator
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public string ThemeDirectory { get; set; }

        private readonly Random _randomNumberGenerator = new Random();

        public WebGenerator()
        {
            Rows = 20;
            Columns = 50;

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

        private string CreateBase()
        {
            string m = "<html><head>";
            m += "<style>" + CssCode + "</style>";
            m += @"<script type=""text/javascript"">" + JqueryCode + @"</script>";
            m += @"
<script type=""text/javascript"">
$(document).ready(function(){
    $('td').click(function(){
            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());
            //railwayEssentialCallback.message('Row: ' + row + ', Column: ' + col);
            railwayEssentialCallback.cellClicked(col, row);
        });
    });

function setCellImage(x, y, src) {
    var selector = '#cell_' + x + '_' + y;
    $(selector).css('background-image','url(' + src + ')');
    railwayEssentialCallback.message('x: ' + x + ', y: ' + y);
    railwayEssentialCallback.message('src: ' + src);
}
</script>";
            m += "<body>\r\n{{CONTENT}}\r\n</body></html>";
            return m;
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
                    var fname = GetNextSvg();
                    var id = "cell_" + x + "_" + y;
                    var title = id;

                    oSb.Append("<td id=\"" + id + "\" class=\"cell\" title=\"" + title + "\" ");
                    oSb.Append("style=\"background-image:url(" + new Uri(fname).AbsoluteUri + ");\"></td>\r\n");
                }

                oSb.Append("</tr>\r\n");
            }

            oSb.Append("</table>");

            try
            {
                var b = CreateBase();
                b = b.Replace("{{CONTENT}}", oSb.ToString());

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
