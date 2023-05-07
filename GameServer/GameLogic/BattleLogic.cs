using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameLogic
{
    public class BattleLogic
    {
        public static double Epsilon = 0.001;


        /// <summary>
        /// Simulate a battle between two fleets
        /// </summary>
        /// <param name="attacker">fleet of attacker</param>
        /// <param name="defender">fleet of defender</param>
        /// <param name="retreatPoint">part of fleet, that should stay alive</param>
        /// <returns>true if attacker won, false otherwise</returns>
        public static bool ArrangeBattle(Fleet attacker, Fleet defender, double retreatPoint = 0.0)
        {
            Force attacker_force = CalcFleetStrength(attacker);
            Force defender_force = CalcFleetStrength(defender);

            double p = Math.Sqrt(attacker_force.power * defender_force.power);
            double r = Math.Sqrt(attacker_force.power / defender_force.power) * attacker_force.count / defender_force.count;
            double time = CalcBattleTime(p, r);

            int attacker_losses = attacker_force.count -
                (int)Math.Floor(CalcBattleResult(attacker_force.count, time, p, 1 / r));
            int defenders_losses = defender_force.count -
                (int)Math.Floor(CalcBattleResult(defender_force.count, time, p, r));

            if (r > 1.0)
            {
                CalcFleetLosses(attacker, attacker_losses);
                defender.RemoveAll();
                return true;
            }
            else
            {
                attacker.RemoveAll();
                CalcFleetLosses(defender, defenders_losses);
                return false;
            }
        }


        struct Force
        {
            public int count;
            public double power;
        }

        private static Force CalcFleetStrength(Fleet fleet)
        {
            Force force = new Force { count = 0, power = 0.0 };

            foreach(var ships in fleet.Ships)
            {
                force.count += ships.Value;
                force.power += Math.Pow(1.5, ships.Key) * fleet.Ships[ships.Key];
            }
            force.power /= force.count;

            return force;
        }


        public static double CalcBattleTime(double first_army, double second_army, double retreat_point)
        {
            double difference = Math.Abs(first_army - second_army);
            if (difference < Epsilon)
            {
                return Math.Log(1 / retreat_point);
            }
            else
            {
                return Math.Log((Math.Sqrt(-(first_army * first_army) + ((first_army * retreat_point) * (first_army * retreat_point)) +
                    second_army * second_army) + (first_army * retreat_point)) / difference);
            }
        }


        public static double CalcBattleTime(double p, double r)
        {
            if (r > 1.0)
                r = 1.0 / r;
            return Math.Log((1 + r) / (1 - r)) / (2.0 * p);
        }


        public static double CalcBattleResult(double first_army, double time, double p, double r)
        {
            return 0.5 * first_army * Math.Pow(Math.E, -time * p) *
                (Math.Pow(Math.E, 2 * p * time) * (1 - r) + 1 + r);
        }


        public static void CalcFleetLosses(Fleet fleet, int losses)
        {
            var keys = fleet.Ships.Keys.ToArray();
            foreach (var lvl in keys)
            {
                if (fleet.Ships[lvl] <= losses)
                {
                    losses -= fleet.Ships[lvl];
                    fleet.Ships.Remove(lvl);
                }
                else
                {
                    fleet.Ships[lvl] -= losses;
                    break;
                }
            }
        }
    }
}
