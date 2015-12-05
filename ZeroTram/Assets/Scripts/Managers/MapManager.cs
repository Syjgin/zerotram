using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Math;
using UnityEngine;

public class MapManager
{
    private static MapManager _instance;

    public static MapManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new MapManager();
        }
        return _instance;
    }

    private Dictionary<string,List<string>> _openedStations;
    private const string OpenedStationsKey = "OpenedStations";
    private const string CurrentWorldKey = "CurrentWorld";
    private string _currentWorldId;
    private readonly JSONObject _map;
    private bool _isNewWorldAnimationNeedToBePlayed;
    private bool _isNewStationAnimationNeedToBePlayed;
    private StationInfo _currentStationInfo;

    private MapManager()
    {
        _map = ConfigReader.GetConfig().GetField("map");
        _openedStations = PersistentDataStorage.GetStringDictionary(OpenedStationsKey);
        _currentWorldId = PlayerPrefs.GetString(CurrentWorldKey, "");
        if (_currentWorldId == string.Empty)
        {
            SetCurrentWorld(_map.keys[0]);
        }
        if (_openedStations.Count == 0)
        {
            OpenNextLevel();
        }
    }

    public void SetCurrentStation(string id)
    {
        _currentStationInfo = GetStationInfo(id);
    }

    public static StationInfo GetStationInfo(string stationId)
    {
        StationInfo info = new StationInfo();
        Dictionary<string, float> passengersMap = new Dictionary<string, float>();
        Dictionary<string, string> unparsedMap = ConfigReader.GetConfig().GetField("levels").GetField(stationId).GetField("passengersMap").ToDictionary();
        foreach (var item in unparsedMap)
        {
            float value = (float)Convert.ToDouble(item.Value);
            passengersMap.Add(item.Key, value);
        }
        info.PassengersMap = passengersMap;
        info.Name = ConfigReader.GetConfig().GetField("levels").GetField(stationId).GetField("name").str;
        info.CheckPointsCount =
            (int)ConfigReader.GetConfig().GetField("levels").GetField(stationId).GetField("count").n;
        return info;
    }

    public string GetCurrentStationName()
    {
        return _currentStationInfo.Name;
    }

    public int GetCurrentCheckPointsCount()
    {
        return _currentStationInfo.CheckPointsCount;
    }

    public string GetRandomCharacter()
    {
        if (_currentStationInfo == null)
        {
            SetCurrentStation(GetDebugLevelName());
        }
        return Randomizer.CalculateValue<string>(_currentStationInfo.PassengersMap);
    }

    public string GetRandomCharacterWithExcludedIndex(string excluded)
    {
        if (_currentStationInfo == null)
        {
            SetCurrentStation(GetDebugLevelName());
        }
        Dictionary<string, float> excludedMap = _currentStationInfo.PassengersMap;
        excludedMap.Remove(excluded);
        return Randomizer.CalculateValue<string>(excludedMap);
    }

    public static string GetDebugLevelName()
    {
        return "level1";
    }

    private void SaveOpenedStations()
    {
        PersistentDataStorage.SaveStringDictionary(OpenedStationsKey, _openedStations);
    }

    public void SetCurrentWorld(string id)
    {
        _currentWorldId = id;
        PlayerPrefs.SetString(CurrentWorldKey, id);
    }
    
    public void OpenNextLevel()
    {
        List<JSONObject> levelIds = _map.GetField(_currentWorldId).list;
        if (_openedStations.ContainsKey(_currentWorldId))
        {
            List<String> openedLevels = _openedStations[_currentWorldId];
            bool allStationsWasOpened = true;
            foreach (var levelId in levelIds)
            {
                string stringRepresentation = levelId.str;
                if (!openedLevels.Contains(stringRepresentation))
                {
                    openedLevels.Add(stringRepresentation);
                    _isNewStationAnimationNeedToBePlayed = true;
                    allStationsWasOpened = false;
                    break;
                }
            }
            if (allStationsWasOpened)
            {
                foreach (var key in _map.keys)
                {
                    if (key != _currentWorldId)
                    {
                        SetCurrentWorld(key);
                        string firstLevelOfNewWorld = _map[key].list[0].str;
                        List<string> listWithFirstLevel = new List<string>();
                        listWithFirstLevel.Add(firstLevelOfNewWorld);
                        _openedStations.Add(key, listWithFirstLevel);
                        _isNewWorldAnimationNeedToBePlayed = true;
                        break;
                    }
                }
            }
        }
        else
        {
            List<string> listWithFirstLevel = new List<string>();
            listWithFirstLevel.Add(levelIds[0].str);
            if (!_openedStations.ContainsKey(_currentWorldId))
            {
                _openedStations.Add(_currentWorldId, listWithFirstLevel);
            }
            _isNewStationAnimationNeedToBePlayed = true;
        }
        SaveOpenedStations();
    }

    public bool IsStationOpened(string stationId)
    {
        return _openedStations[_currentWorldId].Contains(stationId);
    }

    public bool IsNewWorldAnimationNeedToBePlayed()
    {
        return _isNewWorldAnimationNeedToBePlayed;
    }

    public bool IsNewStationAnimationNeedToBePlayed()
    {
        return _isNewStationAnimationNeedToBePlayed;
    }

    public void SetAllAnimationsFinished()
    {
        _isNewStationAnimationNeedToBePlayed = false;
        _isNewWorldAnimationNeedToBePlayed = false;
    }

    public string GetLastOpenedStationId()
    {
        return _openedStations[_currentWorldId].Last();
    }
}
