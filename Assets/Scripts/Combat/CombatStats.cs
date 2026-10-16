using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class CombatStats
{
    [OdinSerialize] public Dictionary<CombatStat, int> stats = new();

    public CombatStats(CombatParticipantSO config)
    {
        stats.Add(CombatStat.STR, config.STR);
        stats.Add(CombatStat.DEF, config.DEF);
        stats.Add(CombatStat.INT, config.INT);
        stats.Add(CombatStat.SPI, config.SPI);
        stats.Add(CombatStat.DEX, config.DEX);
        stats.Add(CombatStat.AGI, config.AGI);
        stats.Add(CombatStat.LUK, config.LUK);

        stats.Add(CombatStat.ATK, 0);
        stats.Add(CombatStat.MATK, 0);
        stats.Add(CombatStat.PDEF, 0);
        stats.Add(CombatStat.MDEF, 0);
        stats.Add(CombatStat.HIT, 0);
        stats.Add(CombatStat.DODGE, 0);
        stats.Add(CombatStat.CRIT, 0);
    }

    public CombatStats()
    {
        stats = new Dictionary<CombatStat, int>();
    }

    public int GetStat(CombatStat stat)
    {
        return stats[stat];
    }

    public CombatStats CopyStats()
    {
        CombatStats copy = new CombatStats();
        foreach(KeyValuePair<CombatStat, int> pair in stats)
        {
            copy.stats.Add(pair.Key, pair.Value);
        }

        return copy;
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
