using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public CombatParticipantSO sample;
    public List<CombatParticipantData> partyMembers = new();
    public static PartyManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AddToParty(sample);
    }

    public List<CombatParticipantData> GetParty() { return  partyMembers; }

    public void AddToParty(CombatParticipantSO newMember)
    {
        partyMembers.Add(new CombatParticipantData(newMember));
    }

    public void UpdateParty(List<CombatParticipantData> partyMembers_) {  partyMembers = partyMembers_; }
}
