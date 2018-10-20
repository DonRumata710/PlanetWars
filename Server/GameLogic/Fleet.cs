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
        public Fleet(int player, Planet planet)
        {
            Owner = player;
            target = planet;
        }

        public Fleet(int player, Dictionary<int, int> _ships, Planet planet)
        {
            Owner = player;
            Ships = _ships;
            target = planet;
        }

        public Fleet(int player, Dictionary<int, int> _ships, Planet planet, double _distance)
        {
            Owner = player;
            Ships = _ships;
            target = planet;
            distance = _distance;
        }


        public void AddShips(int strength, int count)
        {
            if (Ships.ContainsKey(strength))
                Ships[strength] += count;
            else
                Ships.Add(strength, count);
        }

        public void AddShips(Dictionary<int, int> new_ships)
        {
            foreach(var i in new_ships)
            {
                if (Ships.ContainsKey(i.Key))
                    Ships[i.Key] += i.Value;
                else
                    Ships.Add(i.Key, i.Value);
            }
        }

        public int GetShips(int strength)
        {
            if (Ships.ContainsKey(strength))
                return Ships[strength];
            else
                return 0;
        }

        public int RemoveShips(int strength, int count)
        {
            if (Ships.ContainsKey(strength))
            {
                if (Ships[strength] < count)
                {
                    count = Ships[strength];
                    Ships.Remove(strength);
                }
                else
                {
                    Ships[strength] -= count;
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
            Ships.Clear();
        }
        
        public bool IsEmpty()
        {
            return Ships.Count == 0;
        }


        public void Merge(Fleet other)
        {
            foreach(var i in other.Ships)
            {
                if (Ships.ContainsKey(i.Key))
                    Ships[i.Key] += i.Value;
                else
                    Ships.Add(i.Key, i.Value);
            }
            other.RemoveAll();
        }


        public void Attack(Planet planet, double retreat_point)
        {
            if (BattleLogic.ArrangeBattle(this, planet.Guardians))
                planet.Occupate(this);
        }


        public Dictionary<int, int> Ships { get; } = new Dictionary<int, int>();


        public void MakeStep()
        {
            if (distance > 1.0)
            {
                distance -= 1.0;
            }
            else
            {
                distance = 0.0;
                if (target.Owner != Owner)
                    Attack(target, retreat_point);
                else
                    target.AppendArmy(this);
            }

        }


        public int Owner
        {
            get;
            private set;
        }

        Planet target;
        double distance = 0.0;

        double retreat_point = 0.0;
    }
}
