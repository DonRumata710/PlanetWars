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
        public string GetMainPage()
        {
            return File.ReadAllText("html/GameWindow.html");
        }

        public string GetRoomPage()
        {
            return File.ReadAllText("html/Room.html");
        }

        public string GetResourceFile(string filename)
        {
            try
            {
                return File.ReadAllText(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown {0} while file \"{1}\" opening", ex, filename);
                return "";
            }
        }

        public string GetPlayerPage()
        {
            return "";
        }
    }
}
