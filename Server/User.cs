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
                    manager.AddUserToRoom(room_index, this);
                }
                else
                {
                    room_index = Int32.Parse(parameters[0]);
                    manager.AddUserToRoom(room_index, this);
                }

                SetRoomNumber(room_index);
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


        int room_index = -1;
        ConnectionManager manager;
    }
}
