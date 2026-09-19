using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.ComponentModel;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerLabel;
    public GameObject dialogBox;
    public Image portrait;
    public float typingSpeed = 0.02f;
    public InputAction continueDialog;
    public static Action onMessageStart;
    public static Action onMessageComplete;
    private bool inputReceived = false;
    public static DialogBox instance;

    private void Awake()
    {
        continueDialog.Enable();
        continueDialog.performed += ContinueDialog;
        instance = this;

    }

    public void ContinueDialog(InputAction.CallbackContext context)
    {
        inputReceived = true;
    }

    public void ShowMessage(Queue<string> messages)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayMessagePaginated(messages));
    }

    public void ShowMessage(string message, string speaker)
    {
        StopAllCoroutines();
        Queue<string> tmp = new Queue<string>();
        tmp.Enqueue(message);
        StartCoroutine(DisplayMessagePaginated(tmp));
    }

    public void ShowMessage(string message)
    {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        Queue<string> tmp = new Queue<string>();
        tmp.Enqueue(message);
        StartCoroutine(DisplayMessagePaginated(tmp));
    }

    public void ShowMessage(DialogLineData dialogLine)
    {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        Queue<string> tmp = new Queue<string>();
        tmp.Enqueue(dialogLine.dialogLine);
        speakerLabel.text = dialogLine.speaker.characterName;
        StartCoroutine(DisplayMessagePaginated(tmp));
    }

    public void ShowMessage(Queue<string> messages, string speaker)
    {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(DisplayMessagePaginated(messages));
    }

    public void ShowMessage(Queue<DialogLineData> messages)
    {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(DisplayMessagePaginated(messages));
    }

    private IEnumerator DisplayMessagePaginated(Queue<string> messages)
    {
        //PlayerInputController.instance.ToggleControllable(false);
        onMessageStart?.Invoke();
        while (messages.Count > 0)
        {
            dialogText.text = "";
            dialogText.pageToDisplay = 1;

            int previousPageCount = 1;
            string currentMessage = messages.Dequeue();
            for (int i = 0; i < currentMessage.Length; i++)
            {
                dialogText.text += currentMessage[i];
                dialogText.ForceMeshUpdate();

                int currentPageCount = dialogText.textInfo.pageCount;

                if (currentPageCount > previousPageCount)
                {
                    // Page overflow detected
                    inputReceived = false;
                    yield return new WaitUntil(() => inputReceived);

                    // Show the new page
                    dialogText.pageToDisplay = currentPageCount;
                    previousPageCount = currentPageCount;
                }

                yield return new WaitForSeconds(typingSpeed);
            }
            inputReceived = false;
            yield return new WaitUntil(() => inputReceived);

            yield return null;
        }


        // PlayerInputController.instance.ToggleControllable(true);

        onMessageComplete?.Invoke();
        dialogBox.SetActive(false);
        dialogText.text = "";
    }

    private IEnumerator DisplayMessagePaginated(Queue<DialogLineData> messages)
    {
        //PlayerInputController.instance.ToggleControllable(false);
        onMessageStart?.Invoke();

        while (messages.Count > 0)
        {
            dialogText.text = "";
            dialogText.pageToDisplay = 1;

            int previousPageCount = 1;
            DialogLineData currentMessage = messages.Dequeue();
            speakerLabel.text = currentMessage.speaker.characterName;
            for (int i = 0; i < currentMessage.dialogLine.Length; i++)
            {
                dialogText.text += currentMessage.dialogLine[i];
                dialogText.ForceMeshUpdate();

                int currentPageCount = dialogText.textInfo.pageCount;

                if (currentPageCount > previousPageCount)
                {
                    // Page overflow detected
                    inputReceived = false;
                    yield return new WaitUntil(() => inputReceived);

                    // Show the new page
                    dialogText.pageToDisplay = currentPageCount;
                    previousPageCount = currentPageCount;
                }

                yield return new WaitForSeconds(typingSpeed);
            }
            inputReceived = false;
            yield return new WaitUntil(() => inputReceived);

            yield return null;
        }


        //PlayerInputController.instance.ToggleControllable(true);

        onMessageComplete?.Invoke();
        dialogBox.SetActive(false);
        dialogText.text = "";
    }
}
