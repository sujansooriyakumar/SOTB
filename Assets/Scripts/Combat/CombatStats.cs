using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class CombatStats
{
    [OdinSerialize] public Dictionary<CombatStat, int> baseStats = new();
    [OdinSerialize] public Dictionary<CombatStat, int> subStats = new();

    public CombatStats(CombatParticipantSO config)
    {
        baseStats.Add(CombatStat.STR, config.STR);
        baseStats.Add(CombatStat.DEF, config.DEF);
        baseStats.Add(CombatStat.INT, config.INT);
        baseStats.Add(CombatStat.SPI, config.SPI);
        baseStats.Add(CombatStat.DEX, config.DEX);
        baseStats.Add(CombatStat.AGI, config.AGI);
        baseStats.Add(CombatStat.LUK, config.LUK);

        subStats.Add(CombatStat.ATK, 0);
        subStats.Add(CombatStat.MATK, 0);
        subStats.Add(CombatStat.PDEF, 0);
        subStats.Add(CombatStat.MDEF, 0);
        subStats.Add(CombatStat.HIT, 0);
        subStats.Add(CombatStat.DODGE, 0);
        subStats.Add(CombatStat.CRIT, 0);
    }

    public int GetStat(CombatStat stat)
    {
        if (baseStats.ContainsKey(stat))
        {
            return baseStats[stat];
        }
        else
        {
            return subStats[stat];
        }
    }



   
}

public enum CombatStat
{
    STR,
    DEF,
    INT,
    SPI,
    DEX,
    AGI,
    LUK,
    ATK,
    MATK,
    PDEF,
    MDEF,
    HIT,
    DODGE,
    CRIT
}
