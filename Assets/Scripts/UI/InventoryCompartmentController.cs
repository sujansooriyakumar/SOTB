using System;
using System.Collections.Generic;
using UnityEngine;

// Purely visual + state now. Input arrives through UIController -> UIInventoryMenu.HandlePage.
public class InventoryCompartmentController : MonoBehaviour
{
    public List<GameObject> compartmentObjects;
    public float selectedScale = 1.3f;
    public event Action<int> OnCompartmentChanged;

    private int currentIndex;

    public void Step(int direction)
    {
        int count = compartmentObjects.Count;
        if (count == 0) return;

        // Wraps in both directions.
        SetActiveCompartment(((currentIndex + direction) % count + count) % count);
    }

    public void SetActiveCompartment(int index)
    {
        compartmentObjects[currentIndex].transform.localScale = Vector3.one;
        currentIndex = index;
        compartmentObjects[currentIndex].transform.localScale = Vector3.one * selectedScale;
        OnCompartmentChanged?.Invoke(currentIndex);
    }
}