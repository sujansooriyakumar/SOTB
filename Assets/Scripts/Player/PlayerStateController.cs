using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateController : MonoBehaviour
{
    public PlayerState currentState;


    public PlayerState GetCurrentState() { return currentState; }

    public void UpdateState(Vector2 moveDirection)
    {
        switch (moveDirection.x)
        {
            case > 0:
                currentState = PlayerState.RIGHT;
                break;
            case < 0:
                currentState = PlayerState.LEFT;
                break;
        }

        switch (moveDirection.y)
        {
            case > 0:
                currentState = PlayerState.UP;
                break;
            case < 0:
                currentState = PlayerState.DOWN;
                break;
        }

        //GetComponent<PlayerAnimationController>().ForceIdleState(currentState);


    }

    public void UpdateState(PlayerState targetState)
    {
        currentState = targetState;
    }
    public enum PlayerState
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
