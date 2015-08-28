using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Bird : Passenger
    {
        public override void Init()
        {
            MoveProbability = 90;
            AttackProbability = 70;
            ChangeStatePeriod = 5;
            AttackDistance = 2f;
            AttackReloadPeriod = 1;
            AttackMaxDistance = 5;
            CounterAttackProbability = 70;
            Hp = InitialLifes = 150;
            Velocity = 10;
            AttackStrength = 2;
            AttackReactionPeriod = 0.5f;
            TicketProbability = 50;
            StickProbability = 100;
            base.Init();
        }
    }
}
