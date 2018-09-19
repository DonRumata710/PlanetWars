using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.GameLogic
{
    class Coordinates
    {
        public int x = -1;
        public int y = -1;

        Coordinates() { }

        public Coordinates (int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public class EqualityComparer : IEqualityComparer<Coordinates>
        {
            public bool Equals(Coordinates a, Coordinates b)
            {
                return a.x == b.x && a.y == b.y;
            }

            public int GetHashCode(Coordinates a)
            {
                return a.x.GetHashCode() | a.y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return x.ToString() + "-" + y.ToString();
        }
    }


    class Planet
    {
        public Planet(int _size)
        {
            size = _size;
        }

        public override string ToString()
        {
            return "owner=" + owner.ToString() + "size=" + size.ToString() + ",mi=" + military_industry
                + ",ci=" + civil_industry + ",s=" + sience + ";";
        }

        public int GetOwner() => owner;

        public string GetShortInfo()
        {
            string res = "{";

            res += "\"owner\"=" + owner.ToString() + ",\"size\"=" + size.ToString();

            return res + "}";
        }

        public void ChangeOwner(int newOwner)
        {
            owner = newOwner;
        }

        [JsonProperty]
        int size = 0;

        [JsonProperty]
        int military_industry = 0;

        [JsonProperty]
        int civil_industry = 0;

        [JsonProperty]
        int sience = 0;

        [JsonProperty]
        int owner = -1;
    }
}
