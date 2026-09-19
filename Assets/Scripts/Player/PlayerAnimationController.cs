using NUnit.Framework.Internal.Execution;
using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private static string MOVE_X_CONDITION = "moveX";
    private static string MOVE_Y_CONDITION = "moveY";
    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerMovementController.OnMove += UpdateWalkAnimation;
    }
    private void OnDestroy()
    {
        PlayerMovementController.OnMove -= UpdateWalkAnimation;
    }
    private void UpdateWalkAnimation(Vector2 moveVector)
    {
        animator.SetInteger(MOVE_X_CONDITION, (int)moveVector.x);
        animator.SetInteger(MOVE_Y_CONDITION, (int)moveVector.y);
        StartCoroutine(DelayStateChange(moveVector));
    }

    private IEnumerator DelayStateChange(Vector2 moveVector)
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerStateController>().UpdateState(moveVector);
    }
   

    public void ForceIdleState(PlayerStateController.PlayerState direction)
    {
        switch (direction)
        {
            case PlayerStateController.PlayerState.LEFT:
                animator.SetTrigger("idleLeft");
                break;
            case PlayerStateController.PlayerState.RIGHT:
                animator.SetTrigger("idleRight");
                break;
            case PlayerStateController.PlayerState.UP:
                animator.SetTrigger("idleUp");
                break;
            case PlayerStateController.PlayerState.DOWN:
                animator.SetTrigger("idleDown");
                break;
        }
    }
}
