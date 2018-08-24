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
            rooms = new Dictionary<int, Room>(size);

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
            else if (page.Contains(".css") || page.Contains(".js") || page.Contains(".ico"))
            {
                SendPage(e, manager.GetResourceFile(page));
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


        public string CollectRoomInfo()
        {
            string result = "rooms;";
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


        public Room GetRoom(int i) { return rooms[i]; }
        public void AddRoom(int i, int size, int maxPlayers, int planets)
        {
            Monitor.Enter(this);
            rooms.Add(i, new Room(size, maxPlayers, planets));
            RoomUpdate(CollectRoomInfo());
            Monitor.Exit(this);
        }

        public void AddUserToRoom(int i, User user)
        {
            Monitor.Enter(this);
            rooms[i].AddPlayer(user);
            RoomUpdate(CollectRoomInfo());
            Monitor.Exit(this);
        }


        string ip = "http://127.0.0.1:80/";
        Dictionary<int, Room> rooms;
        Dictionary<string, User> players = new Dictionary<string, User>();
        Statistics stat = new Statistics();
    }
}
