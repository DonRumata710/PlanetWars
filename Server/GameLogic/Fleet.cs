using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.GameLogic
{
    public class Fleet
    {
        public Fleet(int player)
        {
            owner = player;
        }

        public Fleet(int player, Dictionary<int, int> _ships)
        {
            owner = player;
            ships = _ships;
        }


        public void AddShips(int strength, int count)
        {
            if (ships.ContainsKey(strength))
                ships[strength] += count;
            else
                ships.Add(strength, count);
        }

        public int GetShips(int strength)
        {
            if (ships.ContainsKey(strength))
                return ships[strength];
            else
                return 0;
        }

        public int RemoveShips(int strength, int count)
        {
            if (ships.ContainsKey(strength))
            {
                if (ships[strength] < count)
                {
                    count = ships[strength];
                    ships.Remove(strength);
                }
                else
                {
                    ships[strength] -= count;
                }
                return count;
            }
            else
            {
                return 0;
            }
        }


        public void RemoveAll()
        {
            ships.Clear();
        }


        public bool IsEmpty()
        {
            return ships.Count == 0;
        }



        public bool Attack(Planet planet, double retreat_point)
        {
            return BattleLogic.ArrangeBattle(this, planet.Guardians);
        }

        
        public Dictionary<int, int> Ships
        {
            get { return ships; }
        }


        [JsonProperty]
        int owner = -1;

        [JsonProperty]
        Dictionary<int, int> ships;
    }
}
