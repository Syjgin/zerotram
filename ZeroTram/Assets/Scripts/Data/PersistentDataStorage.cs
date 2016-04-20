using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PersistentDataStorage
{
    private const char Separator = ';';

    public static void SaveStringDictionary(string key, Dictionary<string, List<string>> dict)
    {
        if(dict.Count == 0)
            return;
        PlayerPrefs.SetString(key, string.Join(Separator.ToString(), dict.Keys.ToArray()));
        foreach (var pair in dict)
        {
            if(pair.Key == string.Empty || pair.Value.Count == 0)
                return;
            PlayerPrefs.SetString(pair.Key, string.Join(Separator.ToString(), pair.Value.ToArray()));
        }
    }

    public static Dictionary<string, List<string>> GetStringDictionary(string key)
    {
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        string[] keys = PlayerPrefs.GetString(key).Split(Separator);
        foreach (var currentKey in keys)
        {
            string unparsed = PlayerPrefs.GetString(currentKey);
            if(unparsed == string.Empty)
                break;
            dict.Add(currentKey, unparsed.Split(Separator).ToList());
        }
        return dict;
    } 
}
