using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{
    private ScrollRect scrollRect;
    private GameObject lastSelected;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        
    }

    private void Update()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if(currentSelected == null || currentSelected == lastSelected)
        {
            return;
        }

        if (currentSelected.transform.IsChildOf(scrollRect.content))
        {
            UpdateScrollPosition(currentSelected.GetComponent<RectTransform>());
            lastSelected = currentSelected;
        }
    }

    private void UpdateScrollPosition(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 localTargetPos = scrollRect.content.InverseTransformPoint(target.position);
        Vector2 currentScrollPos = scrollRect.content.anchoredPosition;

        if (scrollRect.vertical)
        {
            float targetY = -localTargetPos.y - (scrollRect.viewport.rect.height / 2);
            currentScrollPos.y = Mathf.Clamp(targetY, 0, scrollRect.content.rect.height - scrollRect.viewport.rect.height);

        }

        if (scrollRect.horizontal)
        {
            float targetX = -localTargetPos.x - (scrollRect.viewport.rect.width / 2);
            currentScrollPos.x = Mathf.Clamp(targetX, 0, scrollRect.content.rect.width - scrollRect.viewport.rect.width);
        }

        scrollRect.content.anchoredPosition = currentScrollPos;
    }
}
