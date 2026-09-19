using UnityEngine;

public class MapPortal : MonoBehaviour
{
    [SerializeField] private Vector2Int targetCell;
    [SerializeField] private PlayerStateController.PlayerState targetState;

    public Vector2Int GetTargetCell() { return targetCell; }
    public PlayerStateController.PlayerState GetTargetState() { return targetState; }
}
