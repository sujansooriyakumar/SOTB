using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class DialogDisplay : MonoBehaviour
{
    [Header("Event Handler")]
    [SerializeField] private InputSystemUIInputModule uiInputModule;

    [Header("UI References")]
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private Image portrait;
    [SerializeField] private GameObject portraitContainer;
    [SerializeField] private TMP_Text systemDialogText;
    public TMP_Text activeTextBox;
    

    [Header("Display Settings")]
    [SerializeField] private float typingSpeed = 0.02f;
    private bool inputReceived = false;

    [Header("Events")]
    public UnityEvent OnMessageStart;
    public UnityEvent OnMessageEnd;

    public static DialogDisplay Instance;

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
        if (uiInputModule != null)
        {
            if (uiInputModule.cancel != null)
            {
                uiInputModule.cancel.action.performed += AdvanceDialog;
            }

            if (uiInputModule.submit != null)
            {
                uiInputModule.submit.action.performed += AdvanceDialog;
            }
        }
    }

    private void OnDisable()
    {


        if (uiInputModule != null)
        {
            if (uiInputModule.cancel != null)
            {
                uiInputModule.cancel.action.performed -= AdvanceDialog;
            }

            if (uiInputModule.submit != null)
            {
                uiInputModule.submit.action.performed -= AdvanceDialog;
            }
        }
    }

    private void AdvanceDialog(InputAction.CallbackContext c)
    {
        inputReceived = true;
    }

    [ContextMenu("Test Dialog")]
    public void TestDialog()
    {
        DisplaySystemMessage("hello world");
    }

    public void DisplaySystemMessage(string message)
    {
        DialogLineData systemMessage = new DialogLineData();
        systemMessage.speakerID = -1;
        systemMessage.dialogText = message;
        Queue<DialogLineData> queue = new Queue<DialogLineData>();
        queue.Enqueue(systemMessage);
        StartCoroutine(DisplayMessagePaginated(queue));
    }

    private IEnumerator DisplayMessagePaginated(Queue<DialogLineData> messages)
    {
        OnMessageStart?.Invoke();
        while (messages.Count > 0)
        {
           
            DialogLineData currentMessage = messages.Dequeue();
            Debug.Log(currentMessage.dialogText);
            HandleMessageType(currentMessage.speakerID);
            activeTextBox.text = "";
            activeTextBox.pageToDisplay = 1;

            int previousPageCount = 1;
            for (int i = 0; i < currentMessage.dialogText.Length; i++)
            {
                activeTextBox.text += currentMessage.dialogText[i];
                activeTextBox.ForceMeshUpdate();
                int currentPageCount = activeTextBox.textInfo.pageCount;
                if (currentPageCount > previousPageCount)
                {
                    inputReceived = false;
                    yield return new WaitUntil(() => inputReceived);

                    activeTextBox.pageToDisplay = currentPageCount;
                    previousPageCount = currentPageCount;
                }
                yield return new WaitForSeconds(typingSpeed);

            }
            inputReceived = false;
            yield return new WaitUntil(() => inputReceived);

            yield return null;
        }

        OnMessageEnd?.Invoke();
        
    }

    private void HandleMessageType(int speakerID)
    {
        if(speakerID >= 0)
        {
            CharacterSO speaker = CharacterManager.Instance.GetCharacterData(speakerID);
            portraitContainer.SetActive(true);
            portrait.sprite = speaker.characterPortrait;
            speakerText.gameObject.SetActive(true);
            speakerText.text = speaker.characterName;
            systemDialogText.gameObject.SetActive(false);
            dialogText.gameObject.SetActive(true);
            activeTextBox = dialogText;

        }
        else
        {
            portraitContainer.SetActive(false);
            speakerText.gameObject.SetActive(false);
            dialogText.gameObject.SetActive(false);
            systemDialogText.gameObject.SetActive(true);
            activeTextBox = systemDialogText;

        }
    }

}
