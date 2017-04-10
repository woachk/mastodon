using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MastodonUWA
{
    class WebContentHelper
    {
        public static string HtmlHeader(double viewportWidth, double height) //adapt parametres
        {
            var head = new StringBuilder();
            head.Append("<head>");

            head.Append("<meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1, user-scalable=0\"/>");
            head.Append("<script type=\"text/javascript\">" +
                "document.documentElement.style.msScrollTranslation = 'vertical-to-horizontal';" +
                "</script>"); //horizontal scrolling
                              //head.Append("<meta name=\"viewport\" content=\"width=720px\">");
            head.Append("<style>");
            head.Append("html { -ms-text-size-adjust:150%;}");
            head.Append(string.Format("h2{{font-size: 48px}} " +
            "body {{background:white;color:black;font-family:'Segoe UI';font-size:12px;margin:0;padding:0;display: block;" +
            "height: 100%;" +
            "overflow-x: scroll;" +
            "position: relative;" +
            "width: 100%;" +
            "z-index: 0;}}" +
            "article{{column-fill: auto;column-gap: 80px;column-width: 500px; column-height:100%; height:630px;" +
            "}}" +
            "img,p.object,iframe {{ max-width:100%; height:auto }}"));
            head.Append(string.Format("a {{color:blue}}"));
            head.Append("</style>");

            // head.Append(NotifyScript);
            head.Append("</head>");
            return head.ToString();
        }
        public static string WrapHtml(string htmlSubString, double viewportWidth, double height)
        {
            var html = new StringBuilder();
            html.Append("<html>");
            html.Append(HtmlHeader(viewportWidth, height));
            html.Append("<body><article class=\"content\">");
            html.Append(htmlSubString);
            html.Append("</article></body>");
            html.Append("</html>");
            return html.ToString();
        }
    }
}