using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Server
{
    class Room
    {
        public Room(int _id, int _size, int _maxPlayers, int _planets)
        {
            id = _id;
            size = _size;
            maxPlayers = _maxPlayers;

            planets = new Dictionary<GameLogic.Coordinates, GameLogic.Planet>(_planets);

            ThreadSafeRandom rnd = new ThreadSafeRandom();
            for (int i = 0; i < _planets; ++i)
            {
                GameLogic.Coordinates coord;
                do
                    coord = new GameLogic.Coordinates(rnd.Next(size), rnd.Next(size));
                while (planets.ContainsKey(coord));

                planets.Add(coord, new GameLogic.Planet(rnd.Next(size) + 1));
                planets[coord].ChangeOwner(i);
            }
        }

        public void AddPlayer(User user)
        {
            webSockets.Add(user);

            if (webSockets.Count == maxPlayers)
            {
                webSockets[current_player].StartStep();
            }
        }

        public void RemovePlayer(User user)
        {
            webSockets.Remove(user);
            if (webSockets.Count == 0 && GameFinish != null)
                GameFinish(id);
        }

        public override string ToString()
        {
            return String.Format("size={0},players={1},maxplayers={2};", size, webSockets.Count, maxPlayers);
        }

        public bool IsFull()
        {
            return maxPlayers == webSockets.Count;
        }

        public void HandleUserCmd(string cmd)
        {
            string[] cmd_params = cmd.Split(new char[] { ';', '=' });
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

                planets[new GameLogic.Coordinates(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]))].Finance(mil, civ, science);
            }
            else if (cmd_params[0] == "step")
            {
                webSockets[current_player].StopStep();
                current_player = (current_player + 1) % maxPlayers;
                webSockets[current_player].StartStep();
            }
        }


        public string GetMap(int player)
        {
            string description = "planets:{";

            foreach (var planet in planets)
            {
                if (player == -1 || planet.Value.GetOwner() == player)
                    description += "\"" + planet.Key + "\":" + JsonConvert.SerializeObject(planet.Value) + ",";
                else
                    description += "\"" + planet.Key + "\":" + planet.Value.GetShortInfo() + ",";
            }

            return description.Remove(description.Length - 1) + "}";
        }


        public delegate void Update(int id);

        public event Update GameFinish;


        List<User> webSockets = new List<User>();
        private Dictionary<GameLogic.Coordinates, GameLogic.Planet> planets;

        int id = -1;

        int size = 0;

        int maxPlayers = 0;

        int current_player = 0;
    }
}
