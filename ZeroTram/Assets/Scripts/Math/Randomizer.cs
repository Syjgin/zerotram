using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Math
{
    public static class Randomizer
    {
        private const int MaxPercent = 100;
        private static bool _isInitialized;
        public static float GetNormalizedRandom()
        {
            init();
            int percent = Random.Range(0, MaxPercent);
            float result = percent*0.01f;
            return result;
        }

        public static float GetBetweenOneAndMinusOne()
        {
            init();
            int percent = Random.Range(0, MaxPercent*2);
            if (percent%2 == 0)
                percent *= -1;
            float result = percent * 0.01f;
            return result;
        }

        public static int GetRandomPercent()
        {
            init();
            return Random.Range(0, MaxPercent);
        }
        
        public static int GetInRange(int min, int max)
        {
            init();
            return Random.Range(min, max);
        }

        private static void init()
        {
            if (!_isInitialized)
            {
                DateTime utc = DateTime.Now.ToUniversalTime();
                int seed = utc.Year + utc.Month + utc.Hour + utc.Minute + utc.Second;
                Random.seed = seed;
                _isInitialized = true;
            }
        }

        public static bool GetPercentageBasedBoolean(int percent)
        {
            Dictionary<int, bool> values = new Dictionary<int, bool>();
            int remainTrueValues = percent;
            for (int i = 0; i < MaxPercent; i++)
            {
                if (remainTrueValues > 0)
                {
                    values.Add(i, true);
                    remainTrueValues--;
                }
                else
                {
                    values.Add(i, false);
                }
            }
            init();
            int randIndex = Random.Range(0, MaxPercent);
            return values[randIndex];
        }

        public static GameController.BonusTypes CalculateBonus(Dictionary<GameController.BonusTypes, float> bonusMap)
        {
            List<GameController.BonusTypes> values = new List<GameController.BonusTypes>();
            foreach (var mapEntry in bonusMap)
            {
                for (int i = 0; i < mapEntry.Value; i++)
                {
                    values.Add(mapEntry.Key);
                }
            }
            init();
            int randIndex = Random.Range(0, values.Count);
            return values[randIndex];
        }
    }
}
