using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Cat : PassengerSM
    {
        public override void Init()
        {
            AttackProbability = ConfigReader.GetConfig().GetField("cat").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("cat").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("cat").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("cat").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("cat").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("cat").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("cat").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("cat").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("cat").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("cat").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("cat").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("cat").GetField("StickProbability").n;
            base.Init();
        }

        protected override void ShowCharacterInfo()
        {
            if (PlayerPrefs.HasKey(NewCharacterWindow.Prefix + NewCharacterWindow.Character.Cat))
                return;
            Window.SetCharacterToShow(NewCharacterWindow.Character.Cat);
        }
    }
}
