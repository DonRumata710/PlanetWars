using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace Server
{
    class User
    {
        public User(WebSocket socket)
        {
            webSocket = socket;
        }

        public void SetRoomNumber(int num)
        {
            room = num;
        }

        
        int room = -1;
        WebSocket webSocket;
    }
}
