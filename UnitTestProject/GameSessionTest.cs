using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Server;
using Server.GameLogic;


namespace UnitTestProject
{
    [TestClass]
    public class GameSessionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Room room = new Room("", 6, 2, 2);

            Player player1 = new Player();
            Player player2 = new Player();

            int player1_id = room.AddPlayer(player1);
            int player2_id = room.AddPlayer(player2);

            Assert.IsTrue(room.CurrentPlayer == player1_id);

            room.MakeStep(player2_id);

            int income1 = player1.Money;
            int income2 = player2.Money;

            var planet_coords = room.PlanetCoordinates;
            Planet planet1 = room.GetPlanet(planet_coords[0]);
            Planet planet2 = room.GetPlanet(planet_coords[1]);

            int fleet1 = planet1.Guardians.Size;
            int fleet2 = planet2.Guardians.Size;

            for (int i = 0; i < 9; ++i)
            {
                room.MakeStep(player1_id);
                room.MakeStep(player2_id);
            }

            Assert.IsTrue(player1.Money == income1 * 10);
            Assert.IsTrue(player2.Money == income2 * 10);

            Assert.IsTrue(planet1.Guardians.Size == fleet1 * 10);
            Assert.IsTrue(planet2.Guardians.Size == fleet2 * 10);
        }


        [TestMethod]
        public void TestMethod2()
        {
            Room room = new Room("", 6, 2, 2);

            Player player1 = new Player();
            Player player2 = new Player();

            int player1_id = room.AddPlayer(player1);
            int player2_id = room.AddPlayer(player2);


        }
    }
}
