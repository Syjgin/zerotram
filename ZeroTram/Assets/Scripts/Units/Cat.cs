using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Cat : Passenger
    {
        public override void Init(int probability)
        {
            MoveProbability = 10;
            AttackProbability = 20;
            ChangeStatePeriod = 30;
            AttackDistance = 1;
            AttackReloadPeriod = 0.5f;
            AttackMaxDistance = 15;
            CounterAttackProbability = 90;
            Hp = 210;
            Velocity = 5;
            AttackStrength = 7;
            AttackReactionPeriod = 0.5f;
            base.Init(10);
        }
    }
}
