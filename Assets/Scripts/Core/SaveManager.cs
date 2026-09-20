using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System.Linq.Expressions;

public class SaveManager : MonoBehaviour
{
    public PartyManager partyManager;
    public List<CombatParticipantData> party = new();
    private string SavePath => Path.Combine(Application.persistentDataPath, "party.dat");

    public void SavePartyData()
    {
        List<CombatParticipantData> partyMembers = partyManager.partyMembers;
        byte[] bytes = SerializationUtility.SerializeValue(partyMembers, DataFormat.Binary);
        File.WriteAllBytes(SavePath, bytes);
        Debug.Log("Party saved to " + SavePath);

    }
    public void LoadPartyData()
    {
        if (!File.Exists(SavePath))
        {
            return;
        }

        byte[] bytes = File.ReadAllBytes(SavePath);

        party = SerializationUtility.DeserializeValue<List<CombatParticipantData>>(bytes, DataFormat.Binary);
        Debug.Log("Party loaded successfully");

        

    }
}
