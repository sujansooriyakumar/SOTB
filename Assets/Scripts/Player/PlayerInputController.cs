using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    public InputAction walkAction;
    public static event Action<Vector2> OnWalkPerformed;

    private void Awake()
    {
        walkAction.Enable();
        walkAction.performed += WalkInput_Performed;
        walkAction.canceled += WalkInput_Performed;

    }

    public void DisableInput()
    {
        walkAction.Disable();
    }

    public void EnableInput()
    {
        walkAction.Enable();
    }

    public void WalkInput_Performed(InputAction.CallbackContext c)
    {
       
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = walkAction.ReadValue<Vector2>();
        OnWalkPerformed?.Invoke(moveVector);
    }

   
}
