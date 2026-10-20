using System;
using System.Collections.Generic;
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

    private readonly Dictionary<Type, UIMenu> screens = new();
    private Stack<UIMenu>[] layers;
    private readonly HashSet<UIMenu> transitioning = new();
    private bool uiActive;
    private bool menusOpen;

    public static UIController Instance { get; private set; }

    // Raised when the UI starts or stops needing input. Switch action maps here
    // (e.g. disable the Player map while true) instead of checking game state everywhere.
    public static event Action<bool> OnUIActiveChanged;

    // The focused screen: top of the highest non-empty layer.
    public UIMenu Top
    {
        get
        {
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (layers[i].Count > 0) return layers[i].Peek();
            }
            return null;
        }
    }

    // Top of the menu layer, whether or not a modal is covering it.
    public UIMenu CurrentMenu => TopOf(UILayer.Menu);
    public bool HasModal => layers[(int)UILayer.Modal].Count > 0;

    // True while any screen is animating open or closed, including screens already popped.
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

        int layerCount = Enum.GetValues(typeof(UILayer)).Length;
        layers = new Stack<UIMenu>[layerCount];
        for (int i = 0; i < layerCount; i++) layers[i] = new Stack<UIMenu>();

        // Finds every screen under this object, including inactive ones.
        foreach (UIMenu screen in GetComponentsInChildren<UIMenu>(true))
        {
            if (!screens.TryAdd(screen.GetType(), screen))
                Debug.LogError($"Duplicate screen type {screen.GetType().Name} on {screen.name}", screen);

            screen.TransitionStateChanged += OnScreenTransitionChanged;
            screen.gameObject.SetActive(false);
        }
    }

    // ---------- Input: this is the only place UI input is subscribed ----------

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

    // Every action goes to the top screen first. Only cancel has a default if it's unhandled.

    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (IsTransitioning) return;

        UIMenu top = Top;
        if (top != null && !top.HandleCancel()) Close(top);
    }

    private void OnSubmitPressed(InputAction.CallbackContext context)
    {
        if (IsTransitioning) return;
        Top?.HandleSubmit();
    }

    private void OnPagePressed(InputAction.CallbackContext context)
    {
        int direction = Mathf.RoundToInt(context.ReadValue<float>());
        if (direction == 0 || IsTransitioning) return;

        Top?.HandlePage(direction);
    }

    private void OnMenuPressed()
    {
        if (IsTransitioning || HasModal) return;

        if (CurrentMenu == null) Open<UIPauseMenu>();
        else CloseAllMenus();
    }

    private void OnCloseMenuPressed(InputAction.CallbackContext context)
    {
        // Only closes. Opening comes from the Player map through OnMenuPressed.
        if (!IsTransitioning && !HasModal && CurrentMenu != null) CloseAllMenus();
    }

    // ---------- Transitions ----------

    private void OnScreenTransitionChanged(UIMenu screen, bool isTransitioning)
    {
        if (isTransitioning) transitioning.Add(screen);
        else transitioning.Remove(screen);

        // Submit, cancel and navigation through the EventSystem are blocked while anything animates.
        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = !IsTransitioning;
    }

    // ---------- Screens ----------

    public T Get<T>() where T : UIMenu
    {
        if (screens.TryGetValue(typeof(T), out UIMenu screen)) return (T)screen;

        Debug.LogError($"No screen of type {typeof(T).Name} found under {name}");
        return null;
    }

    // Opens a screen on top of its layer. If it's already open, closes everything above it
    // on that layer instead (setup is not run again in that case).
    // Use setup to pass data in before the screen's OnOpen runs.
    public T Open<T>(Action<T> setup = null) where T : UIMenu
    {
        T screen = Get<T>();
        if (screen == null) return null;

        Stack<UIMenu> layer = LayerOf(screen);
        if (layer.Contains(screen))
        {
            ReturnTo(screen);
            return screen;
        }

        // If this screen is still animating closed from a previous use, finish that first
        // so its OnClose cleanup can't wipe out the data setup is about to pass in.
        screen.SkipTransition();
        setup?.Invoke(screen);

        ChangeScreens(() =>
        {
            layer.Push(screen);
            screen.Open();
        });

        return screen;
    }

    // Closes the topmost screen.
    public void Back()
    {
        UIMenu top = Top;
        if (top != null) Close(top);
    }

    // Closes a specific screen, along with anything above it on the same layer.
    public void Close(UIMenu screen)
    {
        if (screen == null) return;

        Stack<UIMenu> layer = LayerOf(screen);
        if (!layer.Contains(screen)) return;

        ChangeScreens(() =>
        {
            UIMenu closed;
            do
            {
                closed = layer.Pop();
                closed.Close();
            }
            while (closed != screen);
        });
    }

    // Closes everything above T on T's layer. Does nothing if T isn't open.
    public void ReturnTo<T>() where T : UIMenu
    {
        T screen = Get<T>();
        if (screen != null) ReturnTo(screen);
    }

    private void ReturnTo(UIMenu target)
    {
        Stack<UIMenu> layer = LayerOf(target);
        if (!layer.Contains(target) || layer.Peek() == target) return;

        ChangeScreens(() =>
        {
            while (layer.Peek() != target)
                layer.Pop().Close();
        });
    }

    // Closes every menu. Modals (dialog, prompts) are left alone.
    public void CloseAllMenus()
    {
        Stack<UIMenu> layer = layers[(int)UILayer.Menu];
        if (layer.Count == 0) return;

        ChangeScreens(() =>
        {
            while (layer.Count > 0)
                layer.Pop().Close();
        });
    }

    // Every stack change goes through here, so focus always follows the top screen
    // no matter which layer changed.
    private void ChangeScreens(Action change)
    {
        UIMenu before = Top;
        change();
        UIMenu after = Top;

        if (before != after && before != null) before.Unfocus(); // no-op if it was just closed
        if (after != null) after.Focus();                       // no-op if already focused

        UpdateState();
    }

    private void UpdateState()
    {
        // Game state only follows the menu layer. A dialog in the overworld doesn't pause it.
        bool anyMenus = layers[(int)UILayer.Menu].Count > 0;
        if (anyMenus != menusOpen)
        {
            menusOpen = anyMenus;
            GameStateManager.ChangeGameState(anyMenus ? GameState.UI : GameState.OVERWORLD);
        }

        bool active = Top != null;
        if (active != uiActive)
        {
            uiActive = active;
            OnUIActiveChanged?.Invoke(active);
        }
    }

    private Stack<UIMenu> LayerOf(UIMenu screen) => layers[(int)screen.Layer];

    private UIMenu TopOf(UILayer layer)
    {
        Stack<UIMenu> stack = layers[(int)layer];
        return stack.Count > 0 ? stack.Peek() : null;
    }
}