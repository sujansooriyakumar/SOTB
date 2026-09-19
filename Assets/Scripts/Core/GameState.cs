using UnityEngine;

public class GameState
{
    // player location
    // party data
    // inventory data
    // quest data
    private Vector2Int playerPosition;

    public Vector2Int GetPlayerPosition() { return playerPosition; }
    public void SetPlayerPosition(Vector2Int position_) {  playerPosition = position_; }
}
