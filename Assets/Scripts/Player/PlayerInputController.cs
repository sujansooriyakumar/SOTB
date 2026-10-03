using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public static event Action<Vector2> OnWalkPerformed;
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
        GameStateManager.OnStateChanged += UpdateOverworldControlState;
    }

    private void UpdateOverworldControlState(GameState gameState)
    {
        if (gameState != GameState.OVERWORLD)
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
        GameStateManager.OnStateChanged -= UpdateOverworldControlState;
    }

    public void MenuInput_Performed(InputAction.CallbackContext c)
    {
        OnMenuPerformed?.Invoke();
    }

    public void InteractInput_Performed(InputAction.CallbackContext c)
    {
        OnInteractPerformed?.Invoke();
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = walkAction.ReadValue<Vector2>();
        OnWalkPerformed?.Invoke(moveVector);
    }

   
}
