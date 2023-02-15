using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public struct Sound
{
    public string key;
    public AudioClip value;
}

[CreateAssetMenu(fileName = "SoundName", menuName = "SoundName", order = 57)]
public class SoundClips : ScriptableObject
{
    public Sound[] soundClipSources;

#if UNITY_EDITOR
    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Substring(0, path.LastIndexOf("/") + 1);
        Debug.Log("SoundNameEnums is updating : " + path);
        StreamWriter writer = new StreamWriter(path + "SoundNameEnums.cs");

        writer.WriteLine("public enum SoundNames{");
        for (int i = 0; i < soundClipSources.Length; i++)
        {
            if (i + 1 <= soundClipSources.Length)
            {
                writer.WriteLine(soundClipSources[i].key + ",");
            }
            else
            {
                writer.WriteLine(soundClipSources[i].key);
            }
        }

        writer.WriteLine("}");
        writer.Close();

    }
#endif
}
