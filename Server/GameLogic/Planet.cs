using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.GameLogic
{
    class Coordinates
    {
        public int x = -1;
        public int y = -1;

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

        int size = 0;
        int military_industry = 0;
        int civil_industry = 0;
        int sience = 0;

        int owner = -1;
    }
}
