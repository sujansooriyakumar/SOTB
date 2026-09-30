using NUnit.Framework.Interfaces;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button tossButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;

    public event Action OnMenuClose;
    public void InitializeContextMenu(InventoryItemData itemData_)
    {
        ItemSO item = InventoryController.Instance.GetItemData(itemData_.uid);
        var (canUse, canEquip, canToss) = item switch
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

    private void SetUpButtonEvents()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    private void SetButtonState(Button button, bool state)
    {
        button.interactable = state;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        text.color = state ? enabledColor : disabledColor;
    }

    private void Cancel()
    {
        cancelButton.onClick.RemoveAllListeners();
        OnMenuClose?.Invoke();
    }
    
}
