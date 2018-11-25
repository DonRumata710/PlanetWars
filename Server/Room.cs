using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Server.GameLogic;


namespace Server
{
    public class Room
    {
        public Room(string _name, int _size, int _maxPlayers, int _planets)
        {
            Name = _name;
            Size = _size;
            MaxPlayers = _maxPlayers;

            planets = new Dictionary<GameLogic.Coordinates, GameLogic.Planet>(_planets);

            ThreadSafeRandom rnd = new ThreadSafeRandom();
            for (int i = 0; i < _planets; ++i)
            {
                GameLogic.Coordinates coord;
                do
                    coord = new GameLogic.Coordinates(rnd.Next(Size), rnd.Next(Size));
                while (planets.ContainsKey(coord));

                planets.Add(coord, new GameLogic.Planet(rnd.Next(Size) + 1, i));
            }
        }

        public int AddPlayer(Player player)
        {
            Monitor.Enter(this);

            NewPlayer?.Invoke(Name);

            players.Add(player);

            if (players.Count == MaxPlayers)
            {
                players[CurrentPlayer].StartStep(CalcPlanetIncome(CurrentPlayer));
            }
            Monitor.Exit(this);

            return players.Count - 1;
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            if (players.Count == 0 && GameFinish != null)
                GameFinish(Name);
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

        public override string ToString()
        {
            return String.Format("name={0},size={1},players={2},maxplayers={3};", Name, Size, players.Count, MaxPlayers);
        }

        public bool IsFull()
        {
            return MaxPlayers == players.Count;
        }


        public void MakeStep(int player)
        {
            players[CurrentPlayer].StopStep();
            CurrentPlayer = (CurrentPlayer + 1) % MaxPlayers;

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
        public int MaxPlayers { get; } = 0;
        public int CurrentPlayer { get; private set; } = 0;
    }
}
