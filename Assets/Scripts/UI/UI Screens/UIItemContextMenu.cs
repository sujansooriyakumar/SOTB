using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemContextMenu : UIMenu
{
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button tossButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;

    private ItemSO itemSelected;
    private InventoryItemData itemDataSelected;
    private Button[] allButtons;

    private void Awake()
    {
        allButtons = new[] { useButton, equipButton, tossButton, cancelButton };

        // The buttons are fixed, so their listeners only need to be added once.
        useButton.onClick.AddListener(Use);
        tossButton.onClick.AddListener(Toss);
        cancelButton.onClick.AddListener(() => UIController.Instance.Back());
    }

    // Called by the inventory before the menu opens.
    public void SetItem(InventoryItemData itemData)
    {
        itemDataSelected = itemData;
        itemSelected = InventoryController.Instance.GetItemData(itemData.uid);
    }

    protected override void OnOpen()
    {
        var (canUse, canEquip, canToss) = itemSelected switch
        {
            ConsumableItemSO => (true, false, true),
            WeaponItemSO or ArmorItemSO => (false, true, true),
            _ => (false, false, false)
        };

        SetButtonState(useButton, canUse);
        SetButtonState(equipButton, canEquip);
        SetButtonState(tossButton, canToss);
        SetButtonState(cancelButton, true);
    }

    protected override void OnClose()
    {
        itemSelected = null;
        itemDataSelected = default;
    }

    protected override GameObject GetDefaultSelection()
    {
        foreach (Button button in allButtons)
        {
            if (button.interactable) return button.gameObject;
        }
        return null;
    }

    private void Use()
    {
        if (itemSelected is ConsumableItemSO consumable)
        {
            UIController.Instance.Open<UIItemTargetSelect>(menu => menu.SetItem(consumable));
        }
    }

    private void Toss()
    {
        // Copies are passed in so the sequence isn't affected if these fields are cleared later.
        StartCoroutine(TossSequence(itemDataSelected, itemSelected));
    }

    // Runs on this menu, which stays open (just unfocused) under the modals until the end.
    private IEnumerator TossSequence(InventoryItemData itemData, ItemSO item)
    {
        // 1. Pick a quantity, skipped when there's only one.
        int amount = 1;
        if (itemData.qty > 1)
        {
            int? picked = null;
            yield return QuantityPicker.Ask(1, itemData.qty, result => picked = result,
                                            prompt: $"How Many?");

            // Cancelled: the picker closes and this menu is focused again.
            if (picked == null) yield break;
            amount = picked.Value;
        }

        string itemLabel = amount > 1 ? $"{item.itemName} x{amount}" : item.itemName;

        // 2. Ask in the dialog box, with the yes/no prompt on top of it.
        yield return DialogDisplay.Instance.ShowPrompt($"Toss {itemLabel}?");

        bool confirmed = false;
        yield return ChoicePrompt.Ask(result => confirmed = result, "Toss", "Keep", defaultYes: false);

        if (!confirmed)
        {
            DialogDisplay.Instance.Hide();
            yield break;
        }

        // 3. Toss, report it in the same dialog box, and go back to the inventory underneath.
        // The inventory rebuilds while covered and regains focus once the dialog closes.
        InventoryController.Instance.RemoveItem(item.uID, amount);
        DialogDisplay.Instance.DisplaySystemMessage($"Tossed {itemLabel}.");
        UIController.Instance.ReturnTo<UIInventoryMenu>();
    }

    private void SetButtonState(Button button, bool state)
    {
        button.interactable = state;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        text.color = state ? enabledColor : disabledColor;
    }
}