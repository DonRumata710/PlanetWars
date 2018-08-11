using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

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

        public void AddPlayer(string name, WebSocket webSocket)
        {
            webSockets.Add(name, webSocket);
        }

        public void RemovePlayer(string name)
        {
            webSockets.Remove(name);
        }

        public override string ToString()
        {
            return String.Format("size={0};players={1};maxplayers={2}", size, players, maxPlayers);
        }

        Dictionary<string, WebSocket> webSockets = new Dictionary<string, WebSocket>();

        int size = 0;
        int planets = 0;

        int maxPlayers = 0;
        int players = 0;

        int ai = 0;
    }
}
