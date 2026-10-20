using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatDisplay : SerializedMonoBehaviour
{

    [OdinSerialize]
    [ShowInInspector]
    public Dictionary<CombatStat, TMP_Text> statDisplays = new();
    private Color defaultColor;

    private CombatStats statReference;

    private void OnEnable()
    {
        defaultColor = statDisplays[CombatStat.STR].color;
    }
    public void UpdateDisplay(CombatStats stats)
    {
        foreach(KeyValuePair<CombatStat, int> kvp in stats.stats)
        {
            statDisplays[kvp.Key].text = kvp.Value.ToString();
            statDisplays[kvp.Key].color = defaultColor;
        }

        statReference = stats;
    }

    public void PreviewStats(CombatStats newStats)
    {
        foreach(KeyValuePair<CombatStat, int> kvp in newStats.stats)
        {
            statDisplays[kvp.Key].text = kvp.Value.ToString();
            if(kvp.Value > statReference.stats[kvp.Key])
            {
                statDisplays[kvp.Key].color = Color.green;
            }
            if (kvp.Value < statReference.stats[kvp.Key])
            {
                statDisplays[kvp.Key].color = Color.red;
            }
            if(kvp.Value == statReference.stats[kvp.Key])
            {
                statDisplays[kvp.Key].color = defaultColor;
            }
        }
    }
}
