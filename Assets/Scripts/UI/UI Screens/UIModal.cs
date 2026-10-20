using System;

// Base for popups that hand a result back to whoever opened them (yes/no, quantity, etc).
// Lives on the Modal layer, above all menus.
public abstract class UIModal<TResult> : UIMenu
{
    private Action<TResult> onSubmit;
    private Action onCancel;

    public override UILayer Layer => UILayer.Modal;

    // Call from the modal's setup, before it opens.
    protected void SetCallbacks(Action<TResult> onSubmit, Action onCancel)
    {
        this.onSubmit = onSubmit;
        this.onCancel = onCancel;
    }

    // Closes the modal, then reports the result. The screen underneath is already
    // refocused when the callback runs, so the callback can safely open other screens.
    protected void Submit(TResult result)
    {
        if (!IsOpen) return;

        Action<TResult> callback = onSubmit;
        ClearCallbacks();
        UIController.Instance.Close(this);
        callback?.Invoke(result);
    }

    protected void Cancel()
    {
        if (!IsOpen) return;

        Action callback = onCancel;
        ClearCallbacks();
        UIController.Instance.Close(this);
        callback?.Invoke();
    }

    // Cancel dismisses the modal by default. Override to treat it as an answer instead.
    public override bool HandleCancel()
    {
        Cancel();
        return true;
    }

    private void ClearCallbacks()
    {
        onSubmit = null;
        onCancel = null;
    }
}