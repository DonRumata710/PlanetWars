using System.Collections.Generic;

namespace GameServer.Models
{
    public class GameState
    {
        public int Size { get; set; }

        public class Planet
        {
            public int Id { get; set; }

            public int x { get; set; }
            public int y { get; set; }

            public int Type { get; set; }
            public int Size { get; set; }
        }

        public List<Planet> Planets { get; set; }
    }
}
