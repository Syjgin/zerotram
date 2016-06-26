using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Client
{
    public static class MessageSender
    {
        public static void SendRewardMessage(JSONObject result, MyMessageScript messageScript, string initialString)
        {
            string rewardString = "";
            if (result.HasField("money"))
            {
                Debug.Log("reward found!");
                float money = result.GetField("money").n;
                string moneyReward = String.Format(StringResources.GetLocalizedString("moneyReward"), money);
                rewardString += moneyReward;
            }
            if (result.HasField("gems"))
            {
                Debug.Log("reward found!");
                float gems = result.GetField("gems").n;
                string gemsReward = String.Format(StringResources.GetLocalizedString("gemsReward"), gems);
                rewardString += gemsReward;
            }
            if (result.HasField("bonus"))
            {
                Debug.Log("reward found!");
                string bonusName = StringResources.GetLocalizedString(result.GetField("bonus").str);
                string bonusReward = String.Format(StringResources.GetLocalizedString("bonusReward"), bonusName);
                rewardString += bonusReward;
            }
            if (result.HasField("tramSkin"))
            {
                Debug.Log("reward found!");
                rewardString += StringResources.GetLocalizedString("tramSkinReward");
            }
            if (result.HasField("conductorSkin"))
            {
                Debug.Log("reward found!");
                rewardString += StringResources.GetLocalizedString("conductorSkinReward");
            }
            if (result.HasField("tramLives"))
            {
                Debug.Log("reward found!");
                float tramLives = result.GetField("tramLives").n;
                rewardString += String.Format(StringResources.GetLocalizedString("tramLivesReward"), tramLives);
            }
            if (rewardString != "")
            {
                initialString += rewardString;
                messageScript.AddMessage(initialString);
            }
        }
    }
}
