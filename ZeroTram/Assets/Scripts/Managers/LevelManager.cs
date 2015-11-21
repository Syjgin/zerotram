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
        ParsePassengersMap();
    }

    public static string GetDebugLevelName()
    {
        return "level1";
    }

    private static void ParsePassengersMap()
    {
        _passengersMap = new Dictionary<string, float>();
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
            SetCurrentLevel(GetDebugLevelName());
        }
        return Randomizer.CalculateValue<string>(_passengersMap);
    }
}
