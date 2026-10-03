using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIController : MonoBehaviour, ICancelHandler
{
    [SerializeField] private InputSystemUIInputModule uiInputModule;

    public GameObject mainMenu;
    public List<UIMenuData> menuData = new();
    private Dictionary<int, UIMenu> menuDictionary = new();
    [SerializeField] private UIMenu currentMenu;
    private Stack<UIMenu> menuHistory = new();

    public static UIController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        PlayerInputController.OnMenuPerformed += OpenMainMenu;
        if (uiInputModule != null && uiInputModule.cancel != null)
        {
            // Subscribe to the EventSystem's global Cancel action
            uiInputModule.cancel.action.performed += OnCancelPressed;
        }

    }

    private void OnDisable()
    {
        PlayerInputController.OnMenuPerformed -= OpenMainMenu;
        if (uiInputModule != null && uiInputModule.cancel != null)
        {
            // Subscribe to the EventSystem's global Cancel action
            uiInputModule.cancel.action.performed -= OnCancelPressed;
        }

    }

    private void OpenMainMenu() { OpenMenu(0); }
    private void OnCancelPressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        CloseMenu();
    }
    private void Start()
    {
        foreach(var item in menuData)
        {
            menuDictionary.Add(item.menuID, item.uiMenu);
        }
    }

 
    public void OpenMenu(int id)
    {
        if(currentMenu != null)
        {
            menuHistory.Push(currentMenu);
            currentMenu.SetLastSelected();
            currentMenu.DeactivateMenu();
            //currentMenu.gameObject.SetActive(false);
        }
 
        UIMenu targetMenu = menuDictionary[id];
        targetMenu.gameObject.SetActive(true);
        targetMenu.ActivateMenu();
        currentMenu = targetMenu;
        GameStateManager.ChangeGameState(GameState.UI);
    }

    public void CloseMenu()
    {
        if (currentMenu == null) return;
        currentMenu.DeactivateMenu();
        currentMenu.CloseMenu();
        if(menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            currentMenu.ActivateMenu();
            currentMenu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(currentMenu.lastSelected);
            return;
        }
        currentMenu = null;
        GameStateManager.ChangeGameState(GameState.OVERWORLD);
    }



    public void OnCancel(BaseEventData eventData)
    {
        CloseMenu();
    }
}

[Serializable]
public class UIMenuData
{
    public UIMenu uiMenu;
    public int menuID;
}
