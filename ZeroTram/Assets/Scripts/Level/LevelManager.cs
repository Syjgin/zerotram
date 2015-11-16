using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using Assets.Scripts.Math;

public static class LevelManager
{
    private static string _currentLevelId;
    private static Dictionary<string, float> _passengersMap; 
    public static void SetCurrentLevel(string levelName)
    {
        _currentLevelId = levelName;
        if (_passengersMap == null)
        {
            _passengersMap = new Dictionary<string, float>();
        }
        ParseBonusMap();
    }

    private static void ParseBonusMap()
    {
        Dictionary<string,string> unparsedMap = ConfigReader.GetConfig().GetField("levels").GetField(_currentLevelId).ToDictionary();
        foreach (var item in unparsedMap)
        {
            float value = (float)Convert.ToDouble(item.Value);
            _passengersMap.Add(item.Key, value);
        }
    }

    public static string GetRandomCharacter()
    {
        if (_passengersMap == null)
        {
            SetCurrentLevel("1_0");
        }
        return Randomizer.CalculateValue<string>(_passengersMap);
    }
}
