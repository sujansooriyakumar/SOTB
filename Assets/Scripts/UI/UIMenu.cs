using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


[RequireComponent(typeof(CanvasGroup))]
public abstract class UIMenu : MonoBehaviour
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

    public event Action<UIMenu, bool> TransitionStateChanged;

    protected GameObject LastSelected => lastSelected;

    protected CanvasGroup Group
    {
        get
        {
            if (canvasGroup == null && !TryGetComponent(out canvasGroup))
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            return canvasGroup;
        }
    }

    // ---------- Lifecycle (called by UIController) ----------

    public void Open()
    {
        SkipTransition(); 

        gameObject.SetActive(true);
        IsOpen = true;
        lastSelected = null;
        Group.interactable = false;

        OnOpen();
        StartTransition(PlayOpenAnimation(), null);
    }


    public void Focus()
    {
        if (IsOpen) ApplyFocus();
    }

    public void Unfocus()
    {
        if (!IsFocused) return;

        lastSelected = EventSystem.current.currentSelectedGameObject;
        Group.interactable = false;
        IsFocused = false;
        OnUnfocus();
    }

    public void Close()
    {
        if (!IsOpen) return;

        SkipTransition(); 

        IsOpen = false;
        IsFocused = false;
        Group.interactable = false;

        StartTransition(PlayCloseAnimation(), FinishClose);
    }

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

        if (animation == null || !animation.MoveNext())
        {
            onComplete?.Invoke();
            return;
        }

        transitionComplete = onComplete;
        Group.blocksRaycasts = false; 
        transition = StartCoroutine(RunTransition(animation));
        TransitionStateChanged?.Invoke(this, true);
    }

    private IEnumerator RunTransition(IEnumerator animation)
    {

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

        if (transition == null) return;

        transition = null;
        transitionComplete = null;
        Group.blocksRaycasts = true;
        TransitionStateChanged?.Invoke(this, false);
    }


    protected virtual IEnumerator PlayOpenAnimation() => Fade(0f, 1f);
    protected virtual IEnumerator PlayCloseAnimation() => Fade(1f, 0f);

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


    protected virtual GameObject GetDefaultSelection() => firstSelected;


    public virtual bool HandleCancel() => false;
    public virtual bool HandlePage(int direction) => false;

    protected virtual void OnOpen() { }
    protected virtual void OnFocus() { }
    protected virtual void OnUnfocus() { }
    protected virtual void OnClose() { }
}