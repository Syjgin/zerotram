using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private string _lastOpenedStationId;
    private readonly JSONObject _map;

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
            OpenNextStation();
        }
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
    
    public void OpenNextStation()
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
                        break;
                    }
                }
            }
        }
        else
        {
            List<string> listWithFirstLevel = new List<string>();
            listWithFirstLevel.Add(levelIds[0].str);
            _openedStations.Add(_currentWorldId, listWithFirstLevel);
        }
        SaveOpenedStations();
    }

    public bool IsStationOpened(string stationId)
    {
        return _openedStations[_currentWorldId].Contains(stationId);
    }
}
