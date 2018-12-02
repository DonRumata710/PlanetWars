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

            Money += new_money;
            is_current = true;

            makeTurn?.Invoke(Money);
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

        public void AddFleet(Fleet fleet)
        {
            fleets.Add(fleet);
        }

        public bool IsCurrent()
        {
            return is_current;
        }

        public int Money { get; private set; } = 0;

        List<Fleet> fleets = new List<Fleet>();

        bool is_current = false;
    }
}
