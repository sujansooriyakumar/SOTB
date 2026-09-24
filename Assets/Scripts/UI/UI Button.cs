using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject selectedObject;

    public void OnDeselect(BaseEventData eventData)
    {
        selectedObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectedObject.SetActive(true);
    }
}
