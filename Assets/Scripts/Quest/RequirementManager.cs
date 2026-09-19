using System.Collections.Generic;
using UnityEngine;

public class RequirementManager : MonoBehaviour
{
    public static RequirementManager instance;
    public Dictionary<RequirementSO, RequirementData> requirementLibrary;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // if needed
    }

   
}
