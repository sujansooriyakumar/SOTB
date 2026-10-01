using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public InputAction pauseAction;
    public InputAction exitAction;
    public GameObject mainMenu;
    public List<UIMenuData> menuData = new();
    private Dictionary<int, UIMenu> menuDictionary = new();
    private UIMenu currentMenu;
    private Queue<UIMenu> menuHistory = new();
    private void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.canceled += OnPausePressed;
        exitAction.Enable();
        exitAction.canceled += ExitPressed;
    }

    private void Start()
    {
        foreach(var item in menuData)
        {
            menuDictionary.Add(item.menuID, item.uiMenu);
        }
    }

    private void ExitPressed(InputAction.CallbackContext c)
    {
        CloseMenu();
    }
    public void OpenMenu(int id)
    {
        if(currentMenu != null)
        {
            menuHistory.Enqueue(currentMenu);
            currentMenu.SetLastSelected();
            currentMenu.DeactivateMenu();
            currentMenu.gameObject.SetActive(false);
        }
 
        UIMenu targetMenu = menuDictionary[id];
        targetMenu.gameObject.SetActive(true);
        targetMenu.ActivateMenu();
        currentMenu = targetMenu;
    }

    public void CloseMenu()
    {
        if (currentMenu == null) return;
        currentMenu.DeactivateMenu();
        currentMenu.gameObject.SetActive(false);
        if(menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Dequeue();
            currentMenu.ActivateMenu();
            currentMenu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(currentMenu.lastSelected);
            return;
        }
        currentMenu = null;
        GameStateManager.ChangeGameState(GameState.OVERWORLD);
    }

    private void OnPausePressed(InputAction.CallbackContext c)
    {
        if (GameStateManager.GetGameState() != GameState.OVERWORLD) return;
        OpenMenu(0);
        GameStateManager.ChangeGameState(GameState.UI);
    }
}

[Serializable]
public class UIMenuData
{
    public UIMenu uiMenu;
    public int menuID;
}
