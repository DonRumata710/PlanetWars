using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interfaces.Models;
using Newtonsoft.Json;

using GameServer.GameLogic;
using GameServer.Models;

namespace GameServer
{
    public class Room
    {
        public Room(Session sessionParameters)
        {
            Name = sessionParameters.Parameters.Name;
            Size = sessionParameters.Parameters.Size;

            planets = new Dictionary<GameLogic.Coordinates, GameLogic.Planet>();

            ThreadSafeRandom rnd = new ThreadSafeRandom();
            for (int i = 0; i < sessionParameters.Parameters.PlanetCount; ++i)
            {
                GameLogic.Coordinates coord;
                do
                    coord = new GameLogic.Coordinates(rnd.Next(Size), rnd.Next(Size));
                while (planets.ContainsKey(coord));

                planets.Add(coord, new GameLogic.Planet(rnd.Next(Size) + 1, i));
            }
        }

        public int GetId(Player player)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (player == players[i])
                    return i;
            }
            return -1;
        }

        public GameState ToGameState()
        {
            GameState state = new GameState();
            state.Size = Size;
            state.Planets = new List<GameState.Planet>(planets.Count);
            foreach(KeyValuePair<Coordinates, Planet> planet in planets)
            {
                GameState.Planet planetState = new GameState.Planet();
                planetState.Size = planet.Value.Size;
                planetState.x = planet.Key.x;
                planetState.y = planet.Key.y;
                planetState.Id = planet.Key.x + planet.Key.y * Size;
                planetState.Type = planet.Value.Size;
                state.Planets.Add(planetState);
            }

            return state;
        }

        public override string ToString()
        {
            return String.Format("name={0},size={1},players={2}", Name, Size, players.Count);
        }


        public void MakeStep(int player)
        {
            players[CurrentPlayer].StopStep();
            CurrentPlayer = (CurrentPlayer + 1) % PlayerNumber;

            players[CurrentPlayer].StartStep(CalcPlanetIncome(CurrentPlayer));
        }


        public string GetMap(int player)
        {
            string description = "map:{";

            foreach (var planet in planets)
            {
                if (player == -1 || planet.Value.Owner == player)
                    description += "\"" + planet.Key + "\":" + JsonConvert.SerializeObject(planet.Value) + ",";
                else
                    description += "\"" + planet.Key + "\":" + planet.Value.GetShortInfo() + ",";
            }

            return description.Remove(description.Length - 1) + "}";
        }


        private int CalcPlanetIncome(int player)
        {
            int totalIncome = 0;
            foreach (var planet in planets)
            {
                if (planet.Value.Owner == player)
                {
                    totalIncome += planet.Value.prepareStep();
                }
            }
            return totalIncome;
        }



        public Planet GetPlanet(int x, int y)
        {
            return GetPlanet(new GameLogic.Coordinates(x, y));
        }

        public Planet GetPlanet(Coordinates coords)
        {
            return planets[coords];
        }

        public Coordinates[] PlanetCoordinates { get { return planets.Keys.ToArray<Coordinates>(); } }


        public delegate void Update(string name);

        public event Update NewPlayer;
        public event Update GameFinish;


        List<Player> players = new List<Player>();
        private Dictionary<GameLogic.Coordinates, GameLogic.Planet> planets;

        public string Name { get; } = "";
        public int Size { get; } = 0;
        public int CurrentPlayer { get; private set; } = 0;
        public int PlayerNumber { get { return players.Count; } }
    }
}
