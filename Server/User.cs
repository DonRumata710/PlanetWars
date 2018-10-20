﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

using DotNet.WebSocket;
using DotNet.WebSocket.Net;
using DotNet.WebSocket.Server;

using Server.GameLogic;

using Newtonsoft.Json;

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

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

            if (room != null)
                room.RemovePlayer(this);
        }

        public void SendRoomInfo(string info)
        {
            SafeSend(info);
        }

        public void SetRoomNumber(int num)
        {
            SafeSend(num.ToString());
        }

        public void StartStep(int new_money)
        {
            foreach (Fleet fleet in fleets)
                fleet.MakeStep();

            money += new_money;
            is_current = true;

            SafeSend("turn:" + money.ToString() + room.GetMap(id));
        }

        public void StopStep()
        {
            is_current = false;

            for (int i = 0; i < fleets.Count; ++i)
            {
                if (fleets[i].IsEmpty())
                {
                    fleets.RemoveAt(i);
                    --i;
                }
            }
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
                    SafeSend(room.GetMap(id));
                }
                else if (is_current)
                {
                    if (e.Data.StartsWith("move:"))
                    {
                        var values = e.Data.Substring(6).Split(new char[] { ':', '>' });
                        Dictionary<int, int> new_fleet_ships = JsonConvert.DeserializeObject<Dictionary<int, int>>(values[0]);

                        Coordinates start_coord = JsonConvert.DeserializeObject<Coordinates>(values[1]);
                        Coordinates target_coord = JsonConvert.DeserializeObject<Coordinates>(values[2]);

                        Planet place = room.GetPlanet(start_coord);
                        Planet target = room.GetPlanet(target_coord);

                        foreach (var ships in new_fleet_ships)
                        {
                            if (place.Guardians.GetShips(ships.Key) < ships.Value)
                                return;
                        }

                        Fleet new_fleet = new Fleet(id, new_fleet_ships, target, Coordinates.Distance(start_coord, target_coord));

                        foreach (var ships in new_fleet_ships)
                            place.Guardians.RemoveShips(ships.Key, ships.Value);
                    }
                    else
                    {
                        room.HandleUserCmd(e.Data);
                    }
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

        List<Fleet> fleets = new List<Fleet>();
        int money = 0;

        bool is_current = false;
    }
}
