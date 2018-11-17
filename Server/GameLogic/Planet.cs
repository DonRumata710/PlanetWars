using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.GameLogic
{
    struct Coordinates
    {
        public int x;
        public int y;

        public static Coordinates Parse(string str)
        {
            string[] coords = str.Split('-');

            Coordinates res = new Coordinates();
            res.x = Int32.Parse(coords[0]);
            res.y = Int32.Parse(coords[1]);

            return res;
        }

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

        static public double Distance(Coordinates first, Coordinates second)
        {
            double x = first.x - second.x;
            double y = first.y - second.y;
            return Math.Sqrt(x * x + y * y);
        }
    }


    public class Planet
    {
        public Planet(int _size, int owner)
        {
            size = _size;
            Owner = owner;
            Guardians = new Fleet(owner, this);
        }

        public override string ToString()
        {
            return "owner=" + Owner.ToString() + "size=" + size.ToString() + ",mi=" + MilitaryIndustryLevel
                + ",ci=" + CivilIndustryLevel + ",s=" + ScienceLevel + ";";
        }

        public string GetShortInfo()
        {
            string res = "{";

            res += "\"Owner\":" + Owner.ToString() + ",\"size\":" + size.ToString();

            return res + "}";
        }


        public void Finance(int military_invest, int civil_invest, int science_invest)
        {
            military_industry += military_invest;
            civil_industry += civil_invest;
            science += science_invest;
        }

        public Fleet Guardians
        {
            get;
            private set;
        }

        public void AppendArmy(Fleet fleet)
        {
            Guardians.Merge(fleet);
        }

        public void Occupate(Fleet occupator)
        {
            Guardians = occupator;
            Owner = occupator.Owner;
        }


        public int prepareStep()
        {
            Guardians.AddShips((int)science, (int)military_industry * size);

            return ((int)civil_industry) * size;
        }


        [JsonProperty]
        int MilitaryIndustryLevel
        {
            get
            {
                return ((int)Math.Ceiling(Math.Log(military_industry))) + 1;
            }
        }

        [JsonProperty]
        int CivilIndustryLevel
        {
            get
            {
                return ((int)Math.Ceiling(Math.Log(civil_industry))) + 1;
            }
        }

        [JsonProperty]
        int ScienceLevel
        {
            get
            {
                return ((int)Math.Ceiling(Math.Log(science))) + 1;
            }
        }

        [JsonProperty]
        public int Owner
        {
            get;
            private set;
        }

        [JsonProperty]
        int size = 0;

        double military_industry = 1.0;

        double civil_industry = 1.0;

        double science = 1.0;
    }
}
