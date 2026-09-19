using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class StorySequenceData
{
    [LabelText("Story Sequence Dictionary")]
    [DictionaryDrawerSettings(KeyLabel = "sequenceID", ValueLabel = "dialogLine", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    [OdinSerialize]
    public Dictionary<int, DialogLineData> sequenceDictionary = new();
    public string sequenceName;
}
