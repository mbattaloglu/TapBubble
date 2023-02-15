using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[System.Serializable]
public struct BoolValue
{
    public string key;
    public bool value;
    public bool defaultValue;
}

[System.Serializable]
public struct IntValue
{
    public string key;
    public int value;
    public int defaultValue;
}

[System.Serializable]
public struct StringValue
{
    public string key;
    public string value;
    public string defaultValue;
}


[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 57)]
public class PlayerData : ScriptableObject
{
    public BoolValue[] boolValues;
    public IntValue[] intValues;
    public StringValue[] stringValues;

    public Grimanda.Common.Avatar activeAvatar;

    public Grimanda.Common.Avatar GetActiveAvatar(Grimanda.Common.Avatar[] avatars)
    {
        string avatarName = PlayerDataUnity.GetString("AvatarName");
        for(int i=0;i<avatars.Length;i++)
        {
            if(avatarName==avatars[i].name)
            {
                return avatars[i];
            }
        }
        return avatars[0];
    }

    public void SetActiveAvatar(Grimanda.Common.Avatar avatar)
    {
        PlayerDataUnity.SetString("AvatarName",avatar.name);
        activeAvatar = avatar;
    }

    public bool GetBool(string key)
    {
        bool defaultValue=false;
        for (int i = 0; i < boolValues.Length; i++)
        {
            if (boolValues[i].key == key)
            {
                defaultValue=boolValues[i].value;
            }
        }

        return PlayerDataUnity.GetBool(key,defaultValue);
    }

    public bool InvertBool(string key)
    {
        PlayerDataUnity.InvertBool(key);
        for (int i = 0; i < boolValues.Length; i++)
        {
            if (boolValues[i].key == key)
            {
                boolValues[i].value = PlayerDataUnity.GetBool(key);
            }
        }


        return PlayerDataUnity.GetBool(key);
    }


    public void SetBool(string key, bool value)
    {
        for(int i=0;i<boolValues.Length;i++)
        {
            if(boolValues[i].key==key)
            {
                boolValues[i].value = value;
            }
        }
        PlayerDataUnity.SetBool(key, value);
    }

    public string GetString(string key)
    {
        string defaultValue = "";
        for (int i = 0; i < stringValues.Length; i++)
        {
            if (intValues[i].key == key)
            {
                defaultValue = stringValues[i].value;
            }
        }

        return PlayerDataUnity.GetString(key, defaultValue);
    }


    public void SetString(string key, string value)
    {
        for (int i = 0; i < stringValues.Length; i++)
        {
            if (stringValues[i].key == key)
            {
                stringValues[i].value = value;
            }
        }
        PlayerDataUnity.SetString(key, value);
    }

    public int GetInt(string key)
    {
        int defaultValue=0;
        for (int i = 0; i < intValues.Length; i++)
        {
            if (intValues[i].key == key)
            {
                defaultValue = intValues[i].value;
            }
        }

        return PlayerDataUnity.GetInt(key, defaultValue);
    }


    public void SetInt(string key, int value)
    {
        for (int i = 0; i < intValues.Length; i++)
        {
            if (intValues[i].key == key)
            {
                intValues[i].value = value;
            }
        }
        PlayerDataUnity.SetInt(key, value);
    }

    public void UpdateInt(string key, int value)
    {
        for (int i = 0; i < intValues.Length; i++)
        {
            if (intValues[i].key == key)
            {
                intValues[i].value = value + PlayerDataUnity.GetInt(key);
            }
        }
        PlayerDataUnity.UpdateInt(key, value);
    }



#if UNITY_EDITOR 
    private void OnValidate()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        if (activeAvatar != null)
        {
            PlayerDataUnity.SetString("AvatarName", activeAvatar.name);
        }

        string path = AssetDatabase.GetAssetPath(this);
        path = path.Substring(0,path.LastIndexOf("/")+1);
        Debug.Log("PlayerDataEnum is updating : " + path); 
        StreamWriter writer = new StreamWriter(path+"PlayerDataTags.cs");

        writer.WriteLine("public static class PlayerDataTags{");
        writer.WriteLine("  public static string AvatarName" + "=\"" + "AvatarName" + "\";");

        for (int i=0;i<boolValues.Length;i++)
        {
            writer.WriteLine("  public static string " + boolValues[i].key + "=\"" + boolValues[i].key +"\";");
            PlayerDataUnity.SetBool(boolValues[i].key, boolValues[i].value);
        }

        for (int i = 0; i < intValues.Length; i++)
        {
            writer.WriteLine("  public static string " + intValues[i].key + "=\"" + intValues[i].key + "\";");
            PlayerDataUnity.SetInt(intValues[i].key, intValues[i].value);
        }

        for (int i = 0; i < stringValues.Length; i++)
        {
            writer.WriteLine("  public static string " + stringValues[i].key + "=\"" + stringValues[i].key + "\";");

            PlayerDataUnity.SetString(stringValues[i].key, stringValues[i].value);

        }

        writer.WriteLine("}");
        writer.Close();

    }
#endif
}
