using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SampleDebug : SerializedMonoBehaviour
{
    
    private void Start()
    {
        GameStateManager.ChangeGameState(GameState.OVERWORLD);
        Debug.Log(GameStateManager.GetGameState());
    }
}
