using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public override void ActivateMenu()
    {
        buttons.Add(useButton);
        buttons.Add(equipButton);
        buttons.Add(tossButton);
        buttons.Add(cancelButton);

        var (canUse, canEquip, canToss) = itemSelected switch
        {
            ConsumableItemSO => (true, false, true),
            WeaponItemSO or ArmorItemSO => (false, true, true),
            QuestItemSO => (false, false, false),
            _ => (false, false, false)
        };

        SetButtonState(useButton, canUse);
        SetButtonState(equipButton, canEquip);
        SetButtonState(tossButton, canToss);

        foreach (var button in new[] { useButton, equipButton, tossButton, cancelButton })
        {
            if (button.interactable)
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }
        SetUpButtonEvents();
    }

    public override void DeactivateMenu()
    {
        useButton.onClick.RemoveAllListeners();
        equipButton.onClick.RemoveAllListeners();
        tossButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    public void GetItemSelected(InventoryItemData itemData_)
    {
        ItemSO itemSelected = InventoryController.Instance.GetItemData(itemData_.uid);

    }

    private void SetUpButtonEvents()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {

        UIController.Instance.CloseMenu();
    }

    private void SetButtonState(Button button, bool state)
    {
        button.interactable = state;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        text.color = state ? enabledColor : disabledColor;
    }





}
