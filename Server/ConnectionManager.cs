using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


using DotNet.WebSocket;
using DotNet.WebSocket.Net;
using DotNet.WebSocket.Server;

namespace Server
{
    class ConnectionManager
    {
        public void Run(int size, string _ip = null)
        {
            rooms = new Dictionary<string, Room>(size);

            if (_ip != null)
                ip = "http://" + _ip + ":80/";
            
            HttpServer server = new HttpServer("http://127.0.0.1:80");
            server.DocumentRootPath = ResourceManager.DocumentRootPath;
            server.OnGet += HttpResponce;

            Action<User> userCreator = delegate (User user) { user.SetConnectionManager(this); };
            server.AddWebSocketService("/auth", userCreator);

            server.Start();
            if (server.IsListening)
                Console.WriteLine("Listening...");
        }


        private void HttpResponce(object sender, HttpRequestEventArgs e)
        {
            string page = e.Request.RawUrl.Substring(1);

            ResourceManager manager = new ResourceManager();

            if (page.IsNullOrEmpty() || page == "main_page")
            {
                SendPage(e, manager.GetMainPage());
            }
            else if (page.Contains(".js") || page.Contains(".html"))
            {
                SendData(e, manager.GetResourceFile(page), "text");
            }
            else if (page.Contains(".css"))
            {
                SendData(e, manager.GetResourceFile(page), "text/css");
            }
            else if (page.Contains(".ico"))
            {
                SendData(e, manager.GetResourceFile(page), "image/ico");
            }
            else if (page.Contains(".png"))
            {
                SendData(e, manager.GetResourceFile(page), "image/png");
            }
            else
            {
                //string key = page.Substring(0, page.IndexOf('?'));
                if (page == "room")
                {
                    SendPage(e, manager.GetRoomPage());
                }
                else if (page == "player")
                {
                    SendPage(e, manager.GetPlayerPage());
                }

            }

            e.Response.Close();
        }


        void SendPage(HttpRequestEventArgs listenerContext, string page)
        {
            listenerContext.Response.StatusCode = page.IsNullOrEmpty() ? 404 : 200;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(page);
            listenerContext.Response.ContentLength64 = buffer.Length;
            Stream output = listenerContext.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }


        void SendData(HttpRequestEventArgs listenerContext, byte[] data, string type)
        {
            listenerContext.Response.StatusCode = data.Length == 0 ? 404 : 200;
            listenerContext.Response.ContentLength64 = data.Length;
            listenerContext.Response.ContentType = type;
            Stream output = listenerContext.Response.OutputStream;
            output.Write(data, 0, data.Length);
            output.Close();
        }


        public string CollectRoomInfo()
        {
            string result = "rooms:";
            foreach (Room room in rooms.Values)
            {
                if (room.IsFull())
                    continue;
                result += room.ToString();
            }
            return result;
        }


        public delegate void Update(string data);

        public event Update RoomUpdate;


        public Room AddRoom(string name, int size, int maxPlayers, int planets)
        {
            Monitor.Enter(this);
            Room new_room = new Room(name, size, maxPlayers, planets);
            rooms.Add(name, new_room);
            RoomUpdate(CollectRoomInfo());
            Monitor.Exit(this);

            new_room.GameFinish += DestroyRoom;
            return new_room;
        }

        public Room AddUserToRoom(string name, User user)
        {
            Monitor.Enter(this);
            rooms[name].AddPlayer(user);
            RoomUpdate(CollectRoomInfo());
            Monitor.Exit(this);

            return rooms[name];
        }

        public void DestroyRoom(string room)
        {
            rooms.Remove(room);
            RoomUpdate(CollectRoomInfo());
            Console.WriteLine(String.Format("Room {0} was destroyed", room));
        }


        string ip = "http://127.0.0.1:80/";
        Dictionary<string, Room> rooms;
        Dictionary<string, User> players = new Dictionary<string, User>();
        Statistics stat = new Statistics();
    }
}
