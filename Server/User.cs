using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

using DotNet.WebSocket;
using DotNet.WebSocket.Net;
using DotNet.WebSocket.Server;


namespace Server
{
    class User : WebSocketBehavior
    {
        public void SetConnectionManager(ConnectionManager cm)
        {
            manager = cm;
            manager.RoomUpdate += SendRoomInfo;
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            id = Statistics.Instance.IncrementConnections();
            SafeSend("id:" + id.ToString());
            SendRoomInfo(manager.CollectRoomInfo());
        }

        public void SendRoomInfo(string info)
        {
            SafeSend(info);
        }

        public void SetRoomNumber(int num)
        {
            SafeSend(num.ToString());
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (room_index == -1)
            {
                string[] parameters = e.Data.Split(new char[2] { '=', ';' });

                if (parameters[0] == "create")
                {
                    room_index = Statistics.Instance.Rooms;
                    Statistics.Instance.IncrementRoomCreations();

                    manager.AddRoom(room_index, Int32.Parse(parameters[2]), Int32.Parse(parameters[4]), Int32.Parse(parameters[6]));
                    room = manager.AddUserToRoom(room_index, this);

                }
                else
                {
                    room_index = Int32.Parse(parameters[0]);
                    room = manager.AddUserToRoom(room_index, this);
                }

                SetRoomNumber(room_index);
            }
            else
            {
                if (e.Data == "getmap")
                {
                    SafeSend(room.GetMap(-1));
                }
                else
                {
                    room.HandleUserCmd(e.Data);
                }
            }
        }

        bool SafeSend(string data)
        {
            if (ConnectionState != DotNet.WebSocket.WebSocketState.Closed)
            {
                Send(data);
                return true;
            }
            return false;
        }


        public string Name { get; set; }


        int id = -1;
        int room_index = -1;
        ConnectionManager manager;
        Room room;
    }
}
