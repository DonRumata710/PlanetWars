using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.GameLogic
{
    public class Player
    {
        public delegate void MakeTurn(int income);
        public event MakeTurn makeTurn;

        public void StartStep(int new_money)
        {
            foreach (Fleet fleet in fleets)
                fleet.MakeStep();

            money += new_money;
            is_current = true;

            makeTurn(money);
            //SafeSend("turn:" + money.ToString() + room.GetMap(id));
        }

        public void StopStep()
        {
            is_current = false;

            for (int i = 0; i < fleets.Count; ++i)
            {
                if (fleets[i].IsEmpty)
                {
                    fleets.RemoveAt(i);
                    --i;
                }
            }
        }

        public bool IsCurrent()
        {
            return is_current;
        }

        List<Fleet> fleets = new List<Fleet>();
        int money = 0;

        bool is_current = false;
    }
}
