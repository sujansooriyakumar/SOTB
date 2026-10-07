using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Forwards directional input from a selected object, with the input module's hold-to-repeat.
// Put it on the selected Selectable and set that Selectable's navigation to None,
// so the direction adjusts a value instead of moving the cursor.
public class MoveEventRelay : MonoBehaviour, IMoveHandler
{
    public event Action<MoveDirection> Moved;

    public void OnMove(AxisEventData eventData)
    {
        Moved?.Invoke(eventData.moveDir);
    }
}