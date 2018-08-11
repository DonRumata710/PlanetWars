using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Statistics
    {
        public void IncrementConnections()
        {
            Monitor.Enter(this);
            ++connections;
            Console.WriteLine("Processed connections: {0}", connections);
            Monitor.Exit(this);
        }

        public void IncrementRoomCreations()
        {
            Monitor.Enter(this);
            ++Rooms;
            Console.WriteLine("Created rooms: {0}", Rooms);
            Monitor.Exit(this);
        }

        public int Rooms { get; private set; } = 0;

        private int connections = 0;
    }
}
