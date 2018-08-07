using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            server.RunAsync(1);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
