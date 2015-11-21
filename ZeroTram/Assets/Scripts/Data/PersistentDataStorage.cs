using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PersistentDataStorage
{
    private const char Separator = ';';
    public static void SaveStringDictionary(String key, Dictionary<string, List<string>> dict)
    {
        PlayerPrefs.SetString(key, String.Join(Separator.ToString(), dict.Keys.ToArray()));
        foreach (var pair in dict)
        {
            PlayerPrefs.SetString(pair.Key, String.Join(Separator.ToString(), pair.Value.ToArray()));
        }
    }

    public static Dictionary<string, List<string>> GetStringDictionary(String key)
    {
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        string[] keys = PlayerPrefs.GetString(key).Split(Separator);
        foreach (var currentKey in keys)
        {
            string unparsed = PlayerPrefs.GetString(currentKey);
            dict.Add(currentKey, unparsed.Split(Separator).ToList());
        }
        return dict;
    } 
}
