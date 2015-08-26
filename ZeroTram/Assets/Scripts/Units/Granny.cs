using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Granny : Passenger
    {
        protected override void Init()
        {
            MoveProbability = 10;
            AttackProbability = 90;
            ChangeStatePeriod = 20;
            AttackDistance = 0.5f;
            AttackReloadPeriod = 2;
            AttackMaxDistance = 10;
            CounterAttackProbability = 90;
            Hp = 170;
            Velocity = 3;
            AttackStrength = 30;
            AttackReactionPeriod = 1;
            CalculateTicket(100);
        }
    }
}
