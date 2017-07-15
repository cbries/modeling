﻿namespace RailwayEssentialWeb
{
    public partial class WebGenerator
    {
        #region CSS Code

        private const string CssCode = @"
/* http://meyerweb.com/eric/tools/css/reset/ 
   v2.0 | 20110126
   License: none (public domain)
*/

html, body, div, span, applet, object, iframe,
h1, h2, h3, h4, h5, h6, p, blockquote, pre,
a, abbr, acronym, address, big, cite, code,
del, dfn, em, img, ins, kbd, q, s, samp,
small, strike, strong, sub, sup, tt, var,
b, u, i, center,
dl, dt, dd, ol, ul, li,
fieldset, form, label, legend,
table, caption, tbody, tfoot, thead, tr, th, td,
article, aside, canvas, details, embed, 
figure, figcaption, footer, header, hgroup, 
menu, nav, output, ruby, section, summary,
time, mark, audio, video {
	margin: 0;
	padding: 0;
	border: 0;
	font-size: 100%;
	font: inherit;
	vertical-align: baseline;
}
/* HTML5 display-role reset for older browsers */
article, aside, details, figcaption, figure, 
footer, header, hgroup, menu, nav, section {
	display: block;
}
body {
	line-height: 1;
}
ol, ul {
	list-style: none;
}
blockquote, q {
	quotes: none;
}
blockquote:before, blockquote:after,
q:before, q:after {
	content: '';
	content: none;
}
table {
	border-collapse: collapse;
	border-spacing: 0;
}

.gridTrackPlan { border: 1px solid black; }
html, body { height: 100%;  }
html { display: table; margin: auto; }
body { display: table-cell; vertical-align: middle; }

.cell {
 width: 32px; height: 32px;
 position: relative;
 overflow: hidden;
}

.cell:before {
  content: """";
  width: 32px; height: 32px;
  background-size: 32px 32px;
  position: absolute;
  left: 0px; top: 0px;
}

.cell4x {
 width: 128px; height: 32px;
 position: relative;
 overflow: hidden;
}

.cell4x:before {
  content: """";
  width: 128px; height: 32px;
  background-size: 128px 32px;
  position: absolute;
  left: 0px; top: 0px;
}
";

        #endregion
    }
}