using UnityEngine;
using UnityEngine.Events;

public class MapNavigation : MonoBehaviour
{
    public static MapNavigation instance;
    [SerializeField] private Grid grid;
    public UnityEvent OnNavigationStart;

    private void Awake()
    {
        instance = this;
    }
    public void GoToCell(Vector2Int targetCell_, Transform player_)
    {
        Vector3 targetPos = grid.GetCellCenterLocal((Vector3Int)targetCell_);
        player_.position = targetPos;
        OnNavigationStart?.Invoke();
    }
}
