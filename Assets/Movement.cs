using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 2f;
    public Animator animator;
    private Rigidbody2D rigidBody;
    private Vector3 direction;

    [Header("Input")]
    public InputAction moveInput;

    public void OnEnable()
    {
        moveInput.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        moveInput.Disable();
    }
    private void Update()
    {
        direction = moveInput.ReadValue<Vector2>();
        AnimateMovement(direction);
    }

    private void FixedUpdate()
    {
      //  this.transform.position += direction * speed * Time.deltaTime;
        rigidBody.linearVelocity = direction * speed;
    }

    private void AnimateMovement(Vector3 direction)
    {
        if (animator == null) return;

        if (direction.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
