using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Server
{
    public class ResourceManager
    {
        static public string DocumentRootPath
        {
            get
            {
                return "html/GameWindow.html";
            }
        }

        static public string RootPath { get; set; }

        public string GetMainPage()
        {
            try
            {
                return File.ReadAllText(RootPath + DocumentRootPath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Main page was not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown {0} while main page was opening", ex);
            }
            return "";
        }

        public string GetRoomPage()
        {
            try
            {
                return File.ReadAllText(RootPath + "html/Room.html");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Room page was not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown {0} while room page was opening", ex);
            }
            return "";
        }

        public byte[] GetResourceFile(string filename)
        {
            filename = RootPath + filename;
            try
            {
                return File.ReadAllBytes(filename);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File {0} was not found", filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown {0} while file \"{1}\" opening", ex, filename);
            }
            return new byte[] { };
        }

        public string GetPlayerPage()
        {
            return "";
        }
    }
}
