using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Statistics
    {
        public void AddConnection()
        {
            Console.WriteLine("A client connected.");
            ++connections;
        }

        int connections = 0;
    }
}
