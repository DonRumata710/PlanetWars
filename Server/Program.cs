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
            ConnectionManager server = new ConnectionManager();
            server.Run(1);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
