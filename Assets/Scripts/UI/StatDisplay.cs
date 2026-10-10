using System.Text;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private TMP_Text STRValue;

    [SerializeField] private TMP_Text INTValue;

    [SerializeField] private TMP_Text DEXValue;

    [SerializeField] private TMP_Text DEFValue;

    [SerializeField] private TMP_Text SPIValue;

    [SerializeField] private TMP_Text AGIValue;

    [SerializeField] private TMP_Text LUKValue;

    [Header("Substats")]
    [SerializeField] private TMP_Text ATKValue;

    [SerializeField] private TMP_Text MATKValue;

    [SerializeField] private TMP_Text PDEFValue;

    [SerializeField] private TMP_Text MDEFValue;

    [SerializeField] private TMP_Text HITValue;

    [SerializeField] private TMP_Text DODGEValue;

    [SerializeField] private TMP_Text CRITValue;

    public void UpdateDisplay(CombatParticipantData combatData)
    {
        CombatStats stats = combatData.stats;

        STRValue.text = stats.GetStat(CombatStat.STR).ToString();
        INTValue.text = stats.GetStat(CombatStat.INT).ToString();
        DEXValue.text = stats.GetStat(CombatStat.DEX).ToString();
        DEFValue.text = stats.GetStat(CombatStat.DEF).ToString();
        SPIValue.text = stats.GetStat(CombatStat.SPI).ToString();
        AGIValue.text = stats.GetStat(CombatStat.AGI).ToString();
        LUKValue.text = stats.GetStat(CombatStat.LUK).ToString();

        // substats
        ATKValue.text = stats.GetStat(CombatStat.ATK).ToString();
        MATKValue.text = stats.GetStat(CombatStat.MATK).ToString();
        PDEFValue.text = stats.GetStat(CombatStat.PDEF).ToString();
        MDEFValue.text = stats.GetStat(CombatStat.MDEF).ToString();
        HITValue.text = stats.GetStat(CombatStat.HIT).ToString();
        DODGEValue.text = stats.GetStat(CombatStat.DODGE).ToString();
        CRITValue.text = stats.GetStat(CombatStat.CRIT).ToString();
    }
}
