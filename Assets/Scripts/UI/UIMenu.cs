using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIMenu:MonoBehaviour
{
    public GameObject lastSelected;
    public abstract void ActivateMenu();
    public abstract void DeactivateMenu();

    public void SetLastSelected()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }

}
