using System;
using UnityEngine;

public static class GameStateManager
{
    public static event Action<GameState> OnStateChanged;
    private static GameState currentState;
    public static void ChangeGameState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }

    public static GameState GetGameState() { return currentState; }
}
