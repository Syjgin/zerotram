using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Alien : Passenger
    {
        protected override void Init()
        {
            MoveProbability = 50;
            AttackProbability = 10;
            ChangeStatePeriod = 5;
            AttackDistance = 0.5f;
            AttackReloadPeriod = 0.5f;
            AttackMaxDistance = 50;
            CounterAttackProbability = 10;
            Hp = 250;
            Velocity = 7;
            AttackStrength = 20;
            AttackReactionPeriod = 1;
            CalculateTicket(80);
        }
    }
}
