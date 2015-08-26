using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Math
{
    public class Randomizer
    {
        private static bool _isInitialized;
        public static float GetNormalizedRandom()
        {
            init();
            int percent = Random.Range(0, 100);
            float result = percent*0.01f;
            return result;
        }

        public static int GetRandomPercent()
        {
            init();
            return Random.Range(0, 100);
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
    }
}
