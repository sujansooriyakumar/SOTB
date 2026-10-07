using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Number picker on the Modal layer. Up/down change the amount by 1 (wrapping),
// left/right by largeStep (clamped). Submit confirms, cancel dismisses.
// Put this on the picker's root panel, under UIController.
public class QuantityPicker : UIModal<int>
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text amountText;
    [Tooltip("The selected object. Submit on it confirms. Needs a MoveEventRelay.")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private MoveEventRelay moveRelay;
    [SerializeField] private int largeStep = 10;

    private string prompt;
    private int min;
    private int max;
    private int amount;

    // Shows the picker. onConfirm receives the chosen amount; onCancel runs if dismissed.
    // Returns false if a picker is already showing or the range is invalid.
    public static bool Show(int min, int max, Action<int> onConfirm, Action onCancel = null,
                            string prompt = "", int? startAmount = null)
    {
        QuantityPicker picker = UIController.Instance.Get<QuantityPicker>();
        if (picker == null) return false;

        if (picker.IsOpen)
        {
            Debug.LogWarning("QuantityPicker is already showing.", picker);
            return false;
        }

        if (max < min)
        {
            Debug.LogWarning($"QuantityPicker range is invalid: {min}-{max}.", picker);
            return false;
        }

        UIController.Instance.Open<QuantityPicker>(p =>
        {
            p.prompt = prompt;
            p.min = min;
            p.max = max;
            p.amount = Mathf.Clamp(startAmount ?? min, min, max);
            p.SetCallbacks(onConfirm, onCancel);
        });
        return true;
    }

    // Coroutine version. onResult gets the amount, or null if cancelled.
    public static IEnumerator Ask(int min, int max, Action<int?> onResult, string prompt = "", int? startAmount = null)
    {
        bool done = false;
        bool shown = Show(min, max,
            amount => { done = true; onResult?.Invoke(amount); },
            () => { done = true; onResult?.Invoke(null); },
            prompt, startAmount);

        if (shown) yield return new WaitUntil(() => done);
    }

    private void Awake()
    {
        confirmButton.onClick.AddListener(() => Submit(amount));
        moveRelay.Moved += OnMoved;

        // Directions change the number instead of moving the cursor.
        confirmButton.navigation = new Navigation { mode = Navigation.Mode.None };
    }

    protected override void OnOpen()
    {
        promptText.text = prompt;
        Refresh();
    }

    protected override GameObject GetDefaultSelection() => confirmButton.gameObject;

    private void OnMoved(MoveDirection direction)
    {
        if (!IsFocused) return;

        switch (direction)
        {
            case MoveDirection.Up: amount = amount >= max ? min : amount + 1; break;
            case MoveDirection.Down: amount = amount <= min ? max : amount - 1; break;
            case MoveDirection.Right: amount = Mathf.Min(amount + largeStep, max); break;
            case MoveDirection.Left: amount = Mathf.Max(amount - largeStep, min); break;
            default: return;
        }

        Refresh();
    }

    private void Refresh()
    {
        amountText.text = amount.ToString();
    }
}