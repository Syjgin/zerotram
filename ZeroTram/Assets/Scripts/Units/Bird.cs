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
            MoveProbability = ConfigReader.GetConfig().GetField("bird").GetField("MoveProbability").n;
            AttackProbability = ConfigReader.GetConfig().GetField("bird").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("bird").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("bird").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("bird").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("bird").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("bird").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("bird").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("bird").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("bird").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("bird").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("bird").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("bird").GetField("StickProbability").n;
            base.Init();
        }
    }
}
