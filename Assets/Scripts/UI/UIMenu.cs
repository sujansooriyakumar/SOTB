using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIMenu:MonoBehaviour
{
    public GameObject lastSelected;
    public List<Button> buttons;
    public virtual void ActivateMenu()
    {
        if (buttons.Count > 0)
        {
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }
        if (lastSelected != null) EventSystem.current.SetSelectedGameObject(lastSelected);
    }
    public virtual void DeactivateMenu()
    {
        if (buttons.Count > 0)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }

    public virtual void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void SetLastSelected()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }

}
