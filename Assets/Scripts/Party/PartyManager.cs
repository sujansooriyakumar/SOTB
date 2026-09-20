using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public CombatParticipantSO sample;
    public List<CombatParticipantData> partyMembers = new();


    public void AddToParty(CombatParticipantSO newMember)
    {
        partyMembers.Add(new CombatParticipantData(newMember));
    }

    public void UpdateParty(List<CombatParticipantData> partyMembers_) {  partyMembers = partyMembers_; }
}
