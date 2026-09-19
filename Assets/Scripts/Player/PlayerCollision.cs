using UnityEngine;
using UnityEngine.UIElements;
using static PlayerStateController;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerStateController stateController;
    [SerializeField] private float collisionDistance = 0.5f;
    [SerializeField] private LayerMask collisionLayer;

    private PlayerAnimationController animationController;
    private Vector2 rayDirection;
    private PlayerMovementController playerMovementController;
    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
        stateController = GetComponent<PlayerStateController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, rayDirection * collisionDistance);
    }
    
    public bool CheckCollision()
    {
        rayDirection = Vector2.zero;

        PlayerState currentState = stateController.currentState;

        switch (currentState)
        {
            case PlayerState.LEFT:
                rayDirection = new Vector2(-1.0f, 0f);
                break;
            case PlayerState.RIGHT:
                rayDirection = new Vector2(1.0f, 0f);
                break;
            case PlayerState.UP:
                rayDirection = new Vector2(0, 1.0f);
                break;
            case PlayerState.DOWN:
                rayDirection = new Vector2(0, -1.0f);
                break;
        }

        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, rayDirection, collisionDistance, collisionLayer);
        
        if(hit && hit.collider.gameObject.TryGetComponent<MapPortal>(out MapPortal p) ){
            MapNavigation.instance.GoToCell(p.GetTargetCell(), transform);
            animationController.ForceIdleState(p.GetTargetState());
            stateController.UpdateState(p.GetTargetState());

        }

        return hit;
    }
}
