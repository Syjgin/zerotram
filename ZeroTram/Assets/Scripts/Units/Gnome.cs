using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Gnome : Passenger
    {
        public override void Init(int probability)
        {
            MoveProbability = 20;
            AttackProbability = 10;
            ChangeStatePeriod = 20;
            AttackDistance = 1;
            AttackReloadPeriod = 2;
            AttackMaxDistance = 2;
            CounterAttackProbability = 70;
            Hp = 600;
            Velocity = 2;
            AttackStrength = 5;
            AttackReactionPeriod = 0.5f;
            base.Init(60);
        }
    }
}
