using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Cat : Passenger
    {
        public override void Init()
        {
            MoveProbability = 10;
            AttackProbability = 50;
            ChangeStatePeriod = 30;
            AttackDistance = 1;
            AttackReloadPeriod = 0.5f;
            AttackMaxDistance = 15;
            CounterAttackProbability = 90;
            Hp = 70;
            Velocity = 5;
            AttackStrength = 7;
            AttackReactionPeriod = 1;
            CalculateTicket(10);
        }
    }
}
