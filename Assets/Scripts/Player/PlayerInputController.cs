using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public static event Action<Vector2> OnWalkPerformed;
    private Vector2 inputVector;

    public static event Action OnMenuPerformed;
    public static event Action OnInteractPerformed;
    InputAction walkAction;
    InputAction interactAction;
    InputAction menuAction;


    private void Awake()
    {

        walkAction = inputAsset.FindAction("Walk");
        interactAction = inputAsset.FindAction("Interact");
        menuAction = inputAsset.FindAction("Menu");

        walkAction.Enable();
        interactAction.Enable();
        menuAction.Enable();

        interactAction.performed += InteractInput_Performed;
        menuAction.performed += MenuInput_Performed;
        GetComponent<PlayerMovementController>().UpdateIsMoving += UpdateActionableState;
        UIController.OnUIActiveChanged += UpdateOverworldControlState;
    }

    public void UpdateActionableState(bool moving)
    {
        if (moving)
        {
            menuAction.Disable();
            interactAction.Disable();
        }
        else
        {
            menuAction.Enable();
            interactAction.Enable();
        }
    }

    private void UpdateOverworldControlState(bool inUI)
    {
        if (inUI)
        {
            walkAction.Disable();
            menuAction.Disable();
            interactAction.Disable();
            return;
        }

        walkAction.Enable();
        menuAction.Enable();
        interactAction.Enable();
    }


    private void OnDestroy()
    {
        interactAction.performed -= InteractInput_Performed;
        menuAction.performed -= MenuInput_Performed;
        UIController.OnUIActiveChanged -= UpdateOverworldControlState;
        GetComponent<PlayerMovementController>().UpdateIsMoving -= UpdateActionableState;

    }

    public void MenuInput_Performed(InputAction.CallbackContext c)
    {
        if (inputVector == Vector2.zero) OnMenuPerformed?.Invoke();
    }

    public void InteractInput_Performed(InputAction.CallbackContext c)
    {
        if (inputVector == Vector2.zero) OnInteractPerformed?.Invoke();
    }

    private void FixedUpdate()
    {
        inputVector = walkAction.ReadValue<Vector2>();
        OnWalkPerformed?.Invoke(inputVector);
    }


}
