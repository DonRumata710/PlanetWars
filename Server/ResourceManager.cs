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
        static public string DocumentRootPath
        {
            get
            {
                return "html/GameWindow.html";
            }
        }

        public string GetMainPage()
        {
            return File.ReadAllText(DocumentRootPath);
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
            catch (FileNotFoundException)
            {
                Console.WriteLine("File {0} was not found", filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown {0} while file \"{1}\" opening", ex, filename);
            }
            return "";
        }

        public string GetPlayerPage()
        {
            return "";
        }
    }
}
