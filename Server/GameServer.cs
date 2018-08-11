using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.IO;

namespace Server
{
    class GameServer
    {
        public async void RunAsync(int size, string _ip = null)
        {
            rooms = new List<Room>(size);

            if (_ip != null)
                ip = "http://" + _ip + ":80/";


            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ip);
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                try
                {
                    HttpListenerContext listenerContext = await listener.GetContextAsync();

                    if (listenerContext.Request.IsWebSocketRequest)
                    {
                        WebSocketResponce(listenerContext);
                    }
                    else
                    {
                        HttpResponce(listenerContext);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);
                }
            }
        }

        private async void WebSocketResponce(HttpListenerContext listenerContext)
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await listenerContext.AcceptWebSocketAsync(subProtocol: null);
                stat.IncrementConnections();
            }
            catch (Exception e)
            {
                listenerContext.Response.StatusCode = 500;
                listenerContext.Response.Close();
                Console.WriteLine("Exception: {0}", e);
                return;
            }

            WebSocket webSocket = webSocketContext.WebSocket;
            string player_id = listenerContext.Request.RemoteEndPoint.ToString();
            players.Add(player_id, new User(webSocket));

            try
            {
                SendRoomInfo(webSocket);

                byte[] receiveBuffer = new byte[1024];
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                string[] parameters = System.Text.Encoding.Default.GetString(receiveBuffer).Split(new char[2]{ '=', ';' });

                if (parameters[0] == "create")
                {
                    int room = stat.Rooms;
                    stat.IncrementRoomCreations();

                    rooms.Add(new Room(Int32.Parse(parameters[2]), Int32.Parse(parameters[4]), Int32.Parse(parameters[6])));
                    rooms[rooms.Count - 1].AddPlayer(player_id, webSocket);
                }
                else
                {
                    rooms[Int32.Parse(parameters[0])].AddPlayer(player_id, webSocket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }


        private void HttpResponce(HttpListenerContext listenerContext)
        {
            string page = listenerContext.Request.RawUrl.Substring(1);

            ResourceManager manager = new ResourceManager();

            if (page == "main_page")
            {
                SendPage(listenerContext, manager.GetMainPage());
            }
            else if (page.Contains(".css") || page.Contains(".js") || page.Contains(".ico"))
            {
                SendPage(listenerContext, manager.GetResourceFile(page));
            }
            else
            {
                string key = page.Substring(0, page.IndexOf('?'));
                if (key == "room")
                {
                    SendPage(listenerContext, manager.GetRoomPage());
                }
                else if (key == "player")
                {
                    SendPage(listenerContext, manager.GetPlayerPage());
                }
                
            }

            listenerContext.Response.StatusCode = 400;
            listenerContext.Response.Close();
        }


        void SendPage(HttpListenerContext listenerContext, string page)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(page);
            listenerContext.Response.ContentLength64 = buffer.Length;
            Stream output = listenerContext.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
        

        async void SendRoomInfo(WebSocket webSocket)
        {
            string result = "";

            foreach (Room room in rooms)
            {
                result += room.ToString();
            }

            byte[] roomInfo = System.Text.Encoding.UTF8.GetBytes(result);
            if (roomInfo.Length > 0)
                await webSocket.SendAsync(new ArraySegment<byte>(roomInfo, 0, roomInfo.Length), WebSocketMessageType.Binary, true, CancellationToken.None);
        }


        string ip = "http://127.0.0.1:80/";
        List<Room> rooms;
        Dictionary<string, User> players = new Dictionary<string, User>();
        Statistics stat = new Statistics();
    }
}
