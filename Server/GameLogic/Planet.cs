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


    public class Planet
    {
        public Planet(int _size)
        {
            size = _size;
        }

        public override string ToString()
        {
            return "owner=" + owner.ToString() + "size=" + size.ToString() + ",mi=" + MilitaryIndustryLevel
                + ",ci=" + CivilIndustryLevel + ",s=" + ScienceLevel + ";";
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


        int BaseDevelopCost = 100;

        public void Finance(int military_invest, int civil_invest, int science_invest)
        {
            military_industry += military_invest;
            civil_industry += civil_invest;
            science += science_invest;
        }

        public Fleet Guardians
        {
            get
            {
                return guardians;
            }
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
        int size = 0;

        double military_industry = 1.0;

        double civil_industry = 1.0;

        double science = 1.0;

        [JsonProperty]
        int owner = -1;

        [JsonProperty]
        Fleet guardians;
    }
}
