using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Grid grid;
    public void LoadGameState(GameState gameState)
    {
        LoadPosition(gameState.GetPlayerPosition());
        LoadParty();
        LoadQuests();
        LoadInventory();
    }

    private void LoadPosition(Vector2Int targetCell)
    {
        player.position = grid.GetCellCenterLocal((Vector3Int)targetCell);
    }

    private void LoadInventory()
    {

    }

    private void LoadParty()
    {

    }

    private void LoadQuests()
    {

    }
}
