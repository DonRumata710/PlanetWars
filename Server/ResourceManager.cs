using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace Server
{
    class ResourceManager
    {
        public string GetMainPage(List<Room> room_list)
        {
            Dictionary<Room, int> room_info = new Dictionary<Room, int>();

            foreach(Room room in room_list)
            {
                ++room_info[room];
            }

            string main_page_string = File.ReadAllText("html/GameWindow.html");

            HtmlParser parser = new HtmlParser();
            IHtmlDocument main_page = parser.Parse(main_page_string);

            //IElement element = main_page.GetElementById("room_list");
            
            return main_page.DocumentElement.OuterHtml;
        }

        public string LoadCss(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
