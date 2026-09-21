using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats
{
    public Dictionary<CombatStat, int> statLibrary = new();

    public CombatStats(CombatParticipantSO config)
    {
       statLibrary.Add(CombatStat.STR, config.STR);
       statLibrary.Add(CombatStat.DEF, config.DEF);
       statLibrary.Add(CombatStat.INT, config.INT);
       statLibrary.Add(CombatStat.SPI, config.SPI);
       statLibrary.Add(CombatStat.DEX, config.DEX);
       statLibrary.Add(CombatStat.AGI, config.AGI);
       statLibrary.Add(CombatStat.LUK, config.LUK);
    }

    public int GetStat(CombatStat stat)
    {
        return statLibrary[stat];
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
    LUK
}
