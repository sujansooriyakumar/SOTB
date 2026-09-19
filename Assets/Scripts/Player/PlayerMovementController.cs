using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerStateController;

public class PlayerMovementController : MonoBehaviour
{
    private Vector2 moveVector;
    [SerializeField] private Grid mapGrid;
    [SerializeField] private float moveDuration = 0.5f;
    private Coroutine movementCoroutine;
    private PlayerStateController stateController;
    public static event Action<Vector2> OnMove;
    private PlayerCollision collisionController;
    public bool isMoving = false;
    private void Awake()
    {
        PlayerInputController.OnWalkPerformed += UpdateMoveVector;
        stateController = GetComponent<PlayerStateController>();
        collisionController = GetComponent<PlayerCollision>();
    }

    private void OnDestroy()
    {
        PlayerInputController.OnWalkPerformed -= UpdateMoveVector;
    }

    private void UpdateMoveVector(Vector2 moveVector_)
    {
        if (Mathf.Abs(moveVector_.x) > Mathf.Abs(moveVector_.y))
        {
            moveVector = new Vector2(Mathf.Sign(moveVector_.x) * 1, 0);
        }

        else if (Mathf.Abs(moveVector_.y) > Mathf.Abs(moveVector_.x))
        {
            moveVector = new Vector2(0, Mathf.Sign(moveVector_.y) * 1);
        }
        else
        {
            moveVector = Vector2.zero;
        }

        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(MovementCoroutine());
        }



    }
    private bool StateCheck()
    {
        PlayerState targetState = PlayerState.LEFT;
        switch (moveVector.x)
        {
            case > 0:
                targetState = PlayerState.RIGHT;
                break;
            case < 0:
                targetState = PlayerState.LEFT;
                break;
        }

        switch (moveVector.y)
        {
            case > 0:
                targetState = PlayerState.UP;
                break;
            case < 0:
                targetState = PlayerState.DOWN;
                break;
        }

        return stateController.GetCurrentState() == targetState;
    }

    private IEnumerator MovementCoroutine()
    {

        while (true)
        {
            // onPlayerMovement?.Invoke(moveDirection);


            Vector3 startPos = transform.position;
            OnMove?.Invoke(moveVector);

            Vector3Int targetCell = mapGrid.LocalToCell(startPos);
            targetCell += new Vector3Int(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), 0);

            Vector3 targetPos = mapGrid.GetCellCenterLocal(targetCell);

            if (StateCheck() && !collisionController.CheckCollision() && moveVector != Vector2.zero)
            {


                float elapsed = 0;
                while (elapsed < moveDuration)
                {
                   
                    elapsed += Time.deltaTime;

                    isMoving = true;

                    float t = elapsed / moveDuration;

                    transform.position = Vector3.Lerp(startPos, targetPos, t);
                    yield return null;


                    transform.position = targetPos;

                }


            }
            else
            {

                movementCoroutine = null;
                yield break;
            }


            yield return null;

        }
    }





}
