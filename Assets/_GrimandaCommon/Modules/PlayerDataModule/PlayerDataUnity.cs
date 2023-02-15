using System;
using UnityEngine;


public class PlayerDataUnity
{
    static public int UpdateInt(string Key, int DeltaValue,int defaultValue=0,int maxValue=999999)
    {
        int _value = GetInt(Key) + DeltaValue;
        Debug.LogError("Updating :" + Key + " " + (_value - DeltaValue).ToString() + " to " + _value);
        if (_value <= maxValue)
        {
            PlayerPrefs.SetInt(Key, _value );
            return (_value);
        }
        else
        {
            PlayerPrefs.SetInt(Key, maxValue);
            return (maxValue);
        }
    }

    public static bool GetBool(string key, bool defaultValue=false)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(key));
        }
        else
        {
            SetBool(key, defaultValue);
            return defaultValue;
        }
    }

    public static void SetBool(string Key, bool value)
    {
        PlayerPrefs.SetInt(Key, Convert.ToInt32(value));
    }

    public static bool InvertBool(string key)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(!GetBool(key)));
        return Convert.ToBoolean(PlayerPrefs.GetInt(key));
    }


    public static void SetInt(string Key,int Value)
    {
        PlayerPrefs.SetInt(Key, Value);
    }


    public static void SetString(string Key, string Value)
    {
        PlayerPrefs.SetString(Key, Value);
    }

    static public int GetInt(string Key,int defaultValue=-666)
    {
        if (PlayerPrefs.HasKey(Key))
        {
            return (PlayerPrefs.GetInt(Key));
        }
        else
        {
            PlayerPrefs.SetInt(Key, defaultValue);
            return (defaultValue);
        }
    }


    static public string GetString(string Key,string defaultValue="defaultValue")
    {
        if (PlayerPrefs.HasKey(Key))
        {
            return (PlayerPrefs.GetString(Key));
        }
        else
        {
            PlayerPrefs.SetString(Key, defaultValue);
            return (defaultValue);
        }
    }
}
