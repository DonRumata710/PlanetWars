using System;
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
    public class User : WebSocketBehavior
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

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            LeaveRoom();
        }

        public void SendRoomInfo(string info)
        {
            SafeSend(info);
        }

        public void SetRoomName(string name)
        {
            SafeSend("room:" + name);
        }

        public void StartStep(int money)
        {
            SafeSend("turn:" + money.ToString() + room.GetMap(id));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (room_name.IsNullOrEmpty())
            {
                string[] parameters = e.Data.Split(new char[2] { '=', ';' });

                if (parameters[0] == "create")
                {
                    room_name = parameters[2];
                    Statistics.Instance.IncrementRoomCreations();

                    manager.AddRoom(parameters[2], Int32.Parse(parameters[4]), Int32.Parse(parameters[6]), Int32.Parse(parameters[8]));
                }
                else
                {
                    room_name = parameters[0];
                }

                room = manager.GetRoom(room_name);
                player = new Player();
                id = room.AddPlayer(player);

                SafeSend("id:" + id.ToString());
                SetRoomName(room_name);
            }
            else
            {
                if (e.Data == "getmap")
                {
                    SafeSend(room.GetMap(id));
                }
                else if (e.Data == "quit")
                {
                    LeaveRoom();
                    SendRoomInfo(manager.CollectRoomInfo());
                }
                else if (player.IsCurrent())
                {
                    if (e.Data.StartsWith("move:"))
                    {
                        var values = e.Data.Substring(5).Split(new char[] { ';', '>' });

                        if (values[1] == values[2])
                            return;

                        Dictionary<int, int> new_fleet_ships = JsonConvert.DeserializeObject<Dictionary<int, int>>(values[0]);

                        Coordinates start_coord = Coordinates.Parse(values[1]);
                        Coordinates target_coord = Coordinates.Parse(values[2]);

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
                        string[] cmd_params = e.Data.Split(new char[] { ';', '=' });
                        if (cmd_params[0] == "finance")
                        {
                            int mil = 0;
                            int civ = 0;
                            int science = 0;
                            string planet = "";

                            for (int i = 1; i < cmd_params.Length; i += 2)
                            {
                                if (cmd_params[i] == "planet")
                                {
                                    planet = cmd_params[i + 1];
                                }
                                else if (cmd_params[i] == "mil")
                                {
                                    mil = Int32.Parse(cmd_params[i + 1]);
                                }
                                else if (cmd_params[i] == "civ")
                                {
                                    civ = Int32.Parse(cmd_params[i + 1]);
                                }
                                else if (cmd_params[i] == "science")
                                {
                                    science = Int32.Parse(cmd_params[i + 1]);
                                }
                            }

                            string[] coordinates = planet.Split(new char[] { '-' });

                            room.GetPlanet(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1])).Finance(mil, civ, science);
                        }
                        else if (cmd_params[0] == "step")
                        {
                            room.MakeStep(id);
                        }
                    }
                }
            }
        }

        public void SetRoom(Room room)
        {
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


        private void LeaveRoom()
        {
            if (room != null)
                room.RemovePlayer(player);

            player = null;
            id = -1;
        }


        public string Name { get; set; }
        public int PlayerId { get { return id; } }


        private int id = -1;
        string room_name = "";
        ConnectionManager manager;
        Room room;
        Player player;
    }
}
