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
        }

        private async void WebSocketResponce(HttpListenerContext listenerContext)
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await listenerContext.AcceptWebSocketAsync(subProtocol: null);
                Interlocked.Increment(ref count);
                Console.WriteLine("Processed: {0}", count);
            }
            catch (Exception e)
            {
                listenerContext.Response.StatusCode = 500;
                listenerContext.Response.Close();
                Console.WriteLine("Exception: {0}", e);
                return;
            }

            WebSocket webSocket = webSocketContext.WebSocket;

            try
            {
                byte[] receiveBuffer = new byte[1024];

                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept text frame", CancellationToken.None);
                    }
                    else
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer, 0, receiveResult.Count), WebSocketMessageType.Binary, receiveResult.EndOfMessage, CancellationToken.None);
                    }                    
                }
            }
            catch (Exception e)
            {
                // Just log any exceptions to the console. Pretty much any exception that occurs when calling `SendAsync`/`ReceiveAsync`/`CloseAsync` is unrecoverable in that it will abort the connection and leave the `WebSocket` instance in an unusable state.
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                // Clean up by disposing the WebSocket once it is closed/aborted.
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }


        private void HttpResponce(HttpListenerContext listenerContext)
        {
            string page = listenerContext.Request.RawUrl;

            ResourceManager manager = new ResourceManager();

            if (page == "/main_page")
            {
                string response = manager.GetMainPage(rooms);

                SendPage(listenerContext, response);
            }
            else if (page.Contains("room"))
            {

            }
            else if (page.Contains("player"))
            {

            }
            else if (page.Contains("css"))
            {
                SendPage(listenerContext, manager.LoadCss(page.Substring(1)));
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


        string ip = "http://127.0.0.1:80/";
        List<Room> rooms;
        Dictionary<string, int> players = new Dictionary<string, int>();
        Statistics stat = new Statistics();

        private int count = 0;
    }
}
