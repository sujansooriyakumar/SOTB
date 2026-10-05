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
    private Button[] allButtons;

    private void Awake()
    {
        allButtons = new[] { useButton, equipButton, tossButton, cancelButton };

        useButton.onClick.AddListener(Use);
        cancelButton.onClick.AddListener(() => UIController.Instance.Back());
    }

    public void SetItem(InventoryItemData itemData)
    {
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

    private void SetButtonState(Button button, bool state)
    {
        button.interactable = state;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        text.color = state ? enabledColor : disabledColor;
    }
}