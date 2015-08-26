﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Bird : Passenger
    {
        protected override void Init()
        {
            MoveProbability = 90;
            AttackProbability = 90;
            ChangeStatePeriod = 5;
            AttackDistance = 2f;
            AttackReloadPeriod = 1;
            AttackMaxDistance = 5;
            CounterAttackProbability = 70;
            Hp = 50;
            Velocity = 7;
            AttackStrength = 2;
            AttackReactionPeriod = 0.5f;
            CalculateTicket(50);
        }
    }
}
