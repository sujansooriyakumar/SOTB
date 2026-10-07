using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject selectedObject;

    public void OnSelect(BaseEventData eventData)
    {
        selectedObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedObject.SetActive(false);
    }

    // OnDeselect never arrives while this object is inactive, so a button that was selected
    // when its menu closed would keep its highlight. Clear it on disable, and on enable show it
    // only if this button really is the current selection.
    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;
        selectedObject.SetActive(eventSystem != null && eventSystem.currentSelectedGameObject == gameObject);
    }

    private void OnDisable()
    {
        if (selectedObject != null) selectedObject.SetActive(false);
    }
}