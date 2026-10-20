using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

// A CanvasGroup lets one flag toggle every Selectable under the menu,
// so menus don't need to keep a list of buttons just to disable them.
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIMenu : SerializedMonoBehaviour
{
    [Tooltip("Selected when the menu opens, or when the remembered selection no longer exists.")]
    [FormerlySerializedAs("firstItem")]
    [SerializeField] private GameObject firstSelected;

    [Tooltip("Used by the default open/close animations. 0 = no animation.")]
    [SerializeField] private float fadeDuration = 0f;

    private CanvasGroup canvasGroup;
    private GameObject lastSelected;

    private Coroutine transition;
    private Action transitionComplete;

    public bool IsOpen { get; private set; }
    public bool IsFocused { get; private set; }
    public bool IsTransitioning => transition != null;

    // Raised with true when an open/close animation starts and false when it ends.
    public event Action<UIMenu, bool> TransitionStateChanged;

    // The selection captured when this menu was last unfocused.
    protected GameObject LastSelected => lastSelected;

    protected CanvasGroup Group
    {
        get
        {
            // Covers menus that had the script attached before RequireComponent was added.
            if (canvasGroup == null && !TryGetComponent(out canvasGroup))
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            return canvasGroup;
        }
    }

    // ---------- Lifecycle (called by UIController) ----------

    // Called once when the menu is pushed onto the stack. UIController calls Focus() right after,
    // so the selection is already in place while the menu animates in.
    public void Open()
    {
        SkipTransition(); // e.g. reopened while its close animation is still playing

        gameObject.SetActive(true);
        IsOpen = true;
        lastSelected = null;
        Group.interactable = false;

        OnOpen();
        StartTransition(PlayOpenAnimation(), null);
    }

    // Called when the menu becomes the top of the stack. Applies immediately, even mid-animation,
    // so the highlight is visible during the fade. Input can't reach the buttons until the
    // animation ends because UIController blocks it while any menu is transitioning.
    public void Focus()
    {
        if (IsOpen) ApplyFocus();
    }

    // Called when another menu or an overlay covers this one.
    public void Unfocus()
    {
        if (!IsFocused) return;

        lastSelected = EventSystem.current.currentSelectedGameObject;
        Group.interactable = false;
        IsFocused = false;
        OnUnfocus();
    }

    // Called once when the menu is popped off the stack. OnClose runs after the close animation.
    public void Close()
    {
        if (!IsOpen) return;

        SkipTransition(); // e.g. closed while its open animation is still playing

        IsOpen = false;
        IsFocused = false;
        Group.interactable = false;

        StartTransition(PlayCloseAnimation(), FinishClose);
    }

    // Jumps any running open/close animation straight to its end state.
    public void SkipTransition()
    {
        if (transition == null) return;

        StopCoroutine(transition);
        EndTransition();
    }

    private void FinishClose()
    {
        OnClose();
        lastSelected = null;
        gameObject.SetActive(false);
    }

    private void ApplyFocus()
    {
        if (IsFocused) return;

        Group.interactable = true;
        IsFocused = true;
        OnFocus();

        bool canRestore = lastSelected != null
                          && lastSelected.activeInHierarchy
                          && lastSelected.transform.IsChildOf(transform);

        EventSystem.current.SetSelectedGameObject(canRestore ? lastSelected : GetDefaultSelection());
    }

    // ---------- Transitions ----------

    private void StartTransition(IEnumerator animation, Action onComplete)
    {
        // Run the first step now. Animations that finish immediately (like the default
        // with fadeDuration = 0) never count as a transition and never block input.
        if (animation == null || !animation.MoveNext())
        {
            onComplete?.Invoke();
            return;
        }

        transitionComplete = onComplete;
        Group.blocksRaycasts = false; // keeps mouse clicks out while animating
        transition = StartCoroutine(RunTransition(animation));
        TransitionStateChanged?.Invoke(this, true);
    }

    private IEnumerator RunTransition(IEnumerator animation)
    {
        // The first step already ran in StartTransition, so this always yields
        // before finishing and can't complete before `transition` is assigned.
        do
        {
            yield return animation.Current;
        }
        while (animation.MoveNext());

        EndTransition();
    }

    private void EndTransition()
    {
        transition = null;
        Group.blocksRaycasts = true;
        Action complete = transitionComplete;
        transitionComplete = null;

        TransitionStateChanged?.Invoke(this, false);
        complete?.Invoke();
    }

    protected virtual void OnDisable()
    {
        // Deactivating the object stops its coroutines. Make sure a transition interrupted
        // that way doesn't leave UIController thinking input should stay blocked.
        // Menus should only be shown and hidden through UIController to avoid this.
        if (transition == null) return;

        transition = null;
        transitionComplete = null;
        Group.blocksRaycasts = true;
        TransitionStateChanged?.Invoke(this, false);
    }

    // Override for custom animations. Set your starting values at the top so an
    // interrupted animation never leaves the next one starting from a half-finished state.
    protected virtual IEnumerator PlayOpenAnimation() => Fade(0f, 1f);
    protected virtual IEnumerator PlayCloseAnimation() => Fade(1f, 0f);

    // Uses unscaled time so menus still animate while the game is paused.
    protected IEnumerator Fade(float from, float to)
    {
        if (fadeDuration <= 0f)
        {
            Group.alpha = to;
            yield break;
        }

        for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            Group.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        Group.alpha = to;
    }

    // ---------- Hooks for subclasses ----------

    protected virtual GameObject GetDefaultSelection() => firstSelected;

    // Which stack this screen lives on. Screens on higher layers sit above and take input first.
    public virtual UILayer Layer => UILayer.Menu;

    // Input routed from UIController to the topmost screen.
    // Return true if the screen handled it. Unhandled cancel falls back to closing the screen.
    // Buttons receive submit through the EventSystem, so most screens leave HandleSubmit alone;
    // it's for screens with nothing to click, like the dialog box.
    public virtual bool HandleSubmit() => false;
    public virtual bool HandleCancel() => false;
    public virtual bool HandlePage(int direction) => false;

    // Build content, subscribe to events (once per open, before the open animation).
    protected virtual void OnOpen() { }
    // Enable menu-specific input, refresh visuals (every time it's on top, at the start of its open animation).
    protected virtual void OnFocus() { }
    // Disable menu-specific input (while covered by another menu).
    protected virtual void OnUnfocus() { }
    // Destroy spawned content, unsubscribe, reset state (once per close, after the close animation).
    protected virtual void OnClose() { }
}