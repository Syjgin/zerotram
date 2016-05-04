using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StringResources
{
    private const String Key = "strings";
    private const String DefaultLangKey = "ru";
    private JSONObject _stringsJsonObject;
    private static StringResources _instance;
    private static String _languageKey;

    private StringResources()
    {
        TextAsset strings = Resources.Load(Key) as TextAsset;
        if (strings != null)
        {
            _stringsJsonObject = new JSONObject(strings.text);
            _stringsJsonObject.Bake();
        }
        else
        {
            _stringsJsonObject = new JSONObject();
        }
    }

    private static String GetLanguageKey()
    {
        if (_languageKey == null)
        {
            _languageKey = DefaultLangKey;
        }
        return _languageKey;
    }

    private static void initIfNeeded()
    {
        if (_instance == null)
        {
            _instance = new StringResources();
        }
    }

    public static void SetLanguage(String keyCode)
    {
        initIfNeeded();
        if (_instance._stringsJsonObject.HasField(keyCode))
        {
            _languageKey = keyCode;
        }
    }

    public static String GetLocalizedString(String key)
    {
        initIfNeeded();
        if (_instance._stringsJsonObject.GetField(GetLanguageKey()).HasField(key))
        {
            return _instance._stringsJsonObject.GetField(GetLanguageKey()).GetField(key).str;
        }
        return "";
    }
}
