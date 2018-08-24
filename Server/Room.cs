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
            planets = _planets;
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
            return String.Format("size={0},players={1},maxplayers={2}", size, players, maxPlayers);
        }

        public bool IsFull()
        {
            return maxPlayers == players;
        }

        List<User> webSockets = new List<User>();

        int size = 0;
        int planets = 0;

        int maxPlayers = 0;
        int players = 0;
    }
}
