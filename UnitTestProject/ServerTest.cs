using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net;
using System.IO;
using System.Threading;

using DotNet.WebSocket;
using DotNet.WebSocket.Net;

using Server;


namespace UnitTestProject
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ServerTest
    {
        public ServerTest()
        {
            ResourceManager.RootPath = "../../../Server/bin/Debug/";
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        const string TEST_SERVER_URL = "localhost";

        private static void StartServer()
        {
            ConnectionManager manager = new ConnectionManager();
            manager.Run(1, TEST_SERVER_URL);
            while (true) ;
        }

        private Thread StartServerThread()
        {
            Thread thread = new Thread(new ThreadStart(StartServer));
            thread.Start();
            return thread;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Thread server_thread = StartServerThread();

            string[] page_list = new string[] {
                "",
                "html/GamePanel.html",
                "html/GameWindow.html",
                "html/RoomListPanel.html",
                "css/Styles.css",
                "js/GameLogic.js",
                "js/util.js"
            };

            foreach (string page in page_list)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + TEST_SERVER_URL + ":80/" + page);
                request.AllowAutoRedirect = false;
                
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
                }
                catch(WebException ex)
                {
                    Assert.Fail("Connection failed with exception: " + ex.Message);
                }
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            Thread server_thread = StartServerThread();

            using (WebSocket socket = new WebSocket("ws://" + TEST_SERVER_URL + ":80/auth"))
            {
                //socket.OnOpen += (sender, e) => socket.Send("Hi, there!");

            }
        }
    }
}
