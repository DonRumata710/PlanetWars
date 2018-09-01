using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Server
{
    class Room
    {
        public Room(int _size, int _maxPlayers, int _planets)
        {
            size = _size;
            maxPlayers = _maxPlayers;
            planets_count = _planets;

            planets = new Dictionary<GameLogic.Coordinates, GameLogic.Planet>(planets_count);
        }

        public void AddPlayer(User user)
        {
            webSockets.Add(user);
        }

        public void RemovePlayer(User user)
        {
            webSockets.Remove(user);
        }

        public override string ToString()
        {
            return String.Format("size={0},players={1},maxplayers={2};", size, webSockets.Count, maxPlayers);
        }

        public bool IsFull()
        {
            return maxPlayers == players;
        }



        public string GetMap()
        {
            string description = "planets:";

            foreach(GameLogic.Planet planet in planets.Values)
                description += planet;

            return description;
        }


        List<User> webSockets = new List<User>();
        private Dictionary<GameLogic.Coordinates, GameLogic.Planet> planets;

        int size = 0;
        int planets_count = 0;

        int maxPlayers = 0;
        int players = 0;
    }
}
