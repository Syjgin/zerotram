using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public static class ConfigReader
{
    private const String ConfigKey = "ConfigKey";
    private static JSONObject _config;


    public static JSONObject GetConfig()
    {
        if (_config == null)
        {
            if (IsLocalConfigAvailable())
            {
                _config = new JSONObject(PlayerPrefs.GetString(ConfigKey));
            }
            else
            {
                TextAsset config = Resources.Load("config") as TextAsset;
                if (config != null)
                {
                    _config = new JSONObject(config.text);
                    _config.Bake();
                }
                else
                {
                    _config = new JSONObject();
                }
            }
        }
        return _config;
    }

    public static String ConfigVersion()
    {
        return GetConfig().GetField("version").str;
    }

    public static void SetConfig(JSONObject config)
    {
        _config = config;
        PlayerPrefs.SetString(ConfigKey, _config.ToString());
    }

    public static bool IsLocalConfigAvailable()
    {
        return PlayerPrefs.HasKey(ConfigKey);
    }
}
