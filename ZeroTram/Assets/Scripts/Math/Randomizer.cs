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
            if (!_isInitialized)
            {
                DateTime utc = DateTime.Now.ToUniversalTime();
                int seed = utc.Year + utc.Month + utc.Hour + utc.Minute + utc.Second;
                Random.seed = seed;
                _isInitialized = true;
            }
            int percent = Random.Range(0, 100);
            float result = percent*0.01f;
            return result;
        }
    }
}
