using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

public class FlagManager : SerializedMonoBehaviour
{
    public static FlagManager Instance;

    [OdinSerialize]
    [ShowInInspector]
    public Dictionary<string, bool> flagDictionary;

    private void Awake()
    {
        if (!Instance) { Instance = this; }
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddFlag(string key_)
    {
        flagDictionary.Add(key_, false);
    }

    public void SetFlag(string key_, bool val_)
    {
        flagDictionary[key_] = val_;
    }

    public bool CheckFlag(string key_)
    {
        return flagDictionary[key_];
    }
}
