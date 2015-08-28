using UnityEngine;
using System.Collections;

public static class ConfigReader
{

    private static JSONObject _config;

    public static JSONObject GetConfig()
    {
        if (_config == null)
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
        return _config;
    }
}
