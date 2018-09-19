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
        static Statistics instance = new Statistics();

        public static Statistics Instance { get { return instance; } }

        public int IncrementConnections()
        {
            Monitor.Enter(this);
            int result = Connections++;
            Console.WriteLine("Processed connections: {0}", Connections);
            Monitor.Exit(this);

            return result;
        }

        public int IncrementRoomCreations()
        {
            Monitor.Enter(this);
            int result = Rooms++;
            Console.WriteLine("Created rooms: {0}", Rooms);
            Monitor.Exit(this);

            return result;
        }

        public int Rooms { get; private set; } = 0;

        public int Connections { get; private set; } = 0;
    }
}
