using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryCompartmentController : MonoBehaviour
{
    public InputAction navigate;
    private int currentIndex;
    public List<GameObject> compartmentObjects;
    public float selectedScale = 1.3f;
    public event Action<int> OnCompartmentChanged;

    private void OnEnable()
    {
        navigate.Enable();
        navigate.performed += NavigateCompartments;
    }

    private void OnDisable()
    {
        navigate.performed -= NavigateCompartments;
        navigate.Disable();

    }

    private void NavigateCompartments(InputAction.CallbackContext c)
    {
        compartmentObjects[currentIndex].transform.localScale = Vector3.one;
        currentIndex += (int)c.ReadValue<float>();
        if(currentIndex > compartmentObjects.Count-1)
        {
            currentIndex = 0;
        }

        if(currentIndex < 0)
        {
            currentIndex = compartmentObjects.Count - 1;
        }
        OnCompartmentChanged?.Invoke(currentIndex);
        compartmentObjects[currentIndex].transform.localScale = Vector3.one * selectedScale;
    }

    public void SetActiveCompartment(int index)
    {
        compartmentObjects[currentIndex].transform.localScale = Vector3.one;
        currentIndex = index;
        OnCompartmentChanged?.Invoke(currentIndex);
        compartmentObjects[currentIndex].transform.localScale = Vector3.one * selectedScale;

    }
}
