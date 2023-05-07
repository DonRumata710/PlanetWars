using GameServer;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace GameServer.Controllers
{
    public class GameManager
    {
        public void AddRoom(int id, Room room)
        {
            rooms.TryAdd(id, room);
        }

        public Room GetRoom(int id)
        {
            return rooms[id];
        }

        private ConcurrentDictionary<int, Room> rooms = new ConcurrentDictionary<int, Room>();
    }
}
