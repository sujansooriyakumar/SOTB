using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputSystemUIInputModule uiInputModule;
    [Tooltip("1D axis in the UI action map, e.g. Q/E or the shoulder buttons. Optional.")]
    [SerializeField] private InputActionReference pageAction;
    [Tooltip("Menu/Start binding in the UI action map, so the menu button still closes menus while the Player map is off. Optional.")]
    [SerializeField] private InputActionReference closeMenuAction;

    private readonly Dictionary<Type, UIMenu> menus = new();
    private readonly Stack<UIMenu> stack = new();
    private readonly List<IUIOverlay> overlays = new();
    private readonly HashSet<UIMenu> transitioning = new();
    private bool uiActive;

    public static UIController Instance { get; private set; }


    public static event Action<bool> OnUIActiveChanged;

    public UIMenu Current => stack.Count > 0 ? stack.Peek() : null;
    public bool HasOverlay => overlays.Count > 0;

    public bool IsTransitioning => transitioning.Count > 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (UIMenu menu in GetComponentsInChildren<UIMenu>(true))
        {
            if (!menus.TryAdd(menu.GetType(), menu))
                Debug.LogError($"Duplicate menu type {menu.GetType().Name} on {menu.name}", menu);

            menu.TransitionStateChanged += OnMenuTransitionChanged;

            menu.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        PlayerInputController.OnMenuPerformed += OnMenuPressed;

        if (uiInputModule != null)
        {
            if (uiInputModule.cancel != null) uiInputModule.cancel.action.performed += OnCancelPressed;
            if (uiInputModule.submit != null) uiInputModule.submit.action.performed += OnSubmitPressed;
        }

        if (pageAction != null)
        {
            pageAction.action.performed += OnPagePressed;
            pageAction.action.Enable();
        }

        if (closeMenuAction != null)
        {
            closeMenuAction.action.performed += OnCloseMenuPressed;
            closeMenuAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        PlayerInputController.OnMenuPerformed -= OnMenuPressed;

        if (uiInputModule != null)
        {
            if (uiInputModule.cancel != null) uiInputModule.cancel.action.performed -= OnCancelPressed;
            if (uiInputModule.submit != null) uiInputModule.submit.action.performed -= OnSubmitPressed;
        }

        if (pageAction != null) pageAction.action.performed -= OnPagePressed;
        if (closeMenuAction != null) closeMenuAction.action.performed -= OnCloseMenuPressed;
    }


    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (IsTransitioning) return;

        if (HasOverlay)
        {
            TopOverlay.HandleCancel();
            return;
        }

        if (Current == null) return;
        if (!Current.HandleCancel()) Back();
    }

    private void OnSubmitPressed(InputAction.CallbackContext context)
    {

        if (IsTransitioning) return;
        if (HasOverlay) TopOverlay.HandleSubmit();
    }

    private void OnPagePressed(InputAction.CallbackContext context)
    {
        int direction = Mathf.RoundToInt(context.ReadValue<float>());
        if (direction == 0 || IsTransitioning || HasOverlay || Current == null) return;

        Current.HandlePage(direction);
    }

    private void OnMenuPressed()
    {
        if (IsTransitioning || HasOverlay) return;

        if (Current == null) Open<UIPauseMenu>();
        else CloseAll();
    }

    private void OnCloseMenuPressed(InputAction.CallbackContext context)
    {
        if (!IsTransitioning && !HasOverlay && Current != null) CloseAll();
    }

    // ---------- Transitions ----------

    private void OnMenuTransitionChanged(UIMenu menu, bool isTransitioning)
    {
        if (isTransitioning) transitioning.Add(menu);
        else transitioning.Remove(menu);


        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = !IsTransitioning;
    }

    // ---------- Overlays ----------

    private IUIOverlay TopOverlay => overlays[overlays.Count - 1];

    public void PushOverlay(IUIOverlay overlay)
    {
        if (overlays.Contains(overlay)) return;

        overlays.Add(overlay);
        if (Current != null) Current.Unfocus();
        UpdateUIActive();
    }

    public void PopOverlay(IUIOverlay overlay)
    {
        if (!overlays.Remove(overlay)) return;

        if (!HasOverlay && Current != null) Current.Focus();
        UpdateUIActive();
    }

    // ---------- Menus ----------

    public T Get<T>() where T : UIMenu
    {
        if (menus.TryGetValue(typeof(T), out UIMenu menu)) return (T)menu;

        Debug.LogError($"No menu of type {typeof(T).Name} found under {name}");
        return null;
    }


    public T Open<T>(Action<T> setup = null) where T : UIMenu
    {
        T menu = Get<T>();
        if (menu == null) return null;

        if (stack.Contains(menu))
        {
            ReturnTo(menu);
            return menu;
        }

        if (Current != null) Current.Unfocus();


        menu.SkipTransition();

        setup?.Invoke(menu);
        stack.Push(menu);
        menu.Open();


        if (!HasOverlay) menu.Focus();

        GameStateManager.ChangeGameState(GameState.UI);
        UpdateUIActive();
        return menu;
    }

    public void Back()
    {
        if (stack.Count == 0) return;

        stack.Pop().Close();

        if (Current != null)
        {
            if (!HasOverlay) Current.Focus();
        }
        else
        {
            GameStateManager.ChangeGameState(GameState.OVERWORLD);
        }

        UpdateUIActive();
    }

    public void ReturnTo<T>() where T : UIMenu
    {
        T menu = Get<T>();
        if (menu != null) ReturnTo(menu);
    }

    private void ReturnTo(UIMenu target)
    {
        if (Current == target || !stack.Contains(target)) return;

        while (stack.Peek() != target)
            stack.Pop().Close();

        if (!HasOverlay) target.Focus();
    }

    public void CloseAll()
    {
        while (stack.Count > 0)
            stack.Pop().Close();

        GameStateManager.ChangeGameState(GameState.OVERWORLD);
        UpdateUIActive();
    }

    private void UpdateUIActive()
    {
        bool active = stack.Count > 0 || HasOverlay;
        if (active == uiActive) return;

        uiActive = active;
        OnUIActiveChanged?.Invoke(active);
    }
}