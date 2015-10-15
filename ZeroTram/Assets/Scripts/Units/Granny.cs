using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Granny : PassengerSM
    {
        public override void Init()
        {
            AttackProbability = ConfigReader.GetConfig().GetField("granny").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("granny").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("granny").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("granny").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("granny").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("granny").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("granny").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("granny").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("granny").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("granny").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("granny").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("granny").GetField("StickProbability").n;
            base.Init();
        }

        protected override void ShowCharacterInfo()
        {
            if (PlayerPrefs.HasKey(NewCharacterWindow.Prefix + NewCharacterWindow.Character.Granny))
                return;
            Window.SetCharacterToShow(NewCharacterWindow.Character.Granny);
        }
    }
}
