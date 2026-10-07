using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Dialog box on the Modal layer. Put this on the dialog's root panel, under UIController.
public class DialogDisplay : UIMenu
{
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

    private readonly Queue<DialogLineData> lines = new();
    private Coroutine running;

    [Header("Events")]
    public UnityEvent OnMessageStart;
    public UnityEvent OnMessageEnd;

    // Looked up through UIController, so it works before this object has ever been active.
    // UIController's DontDestroyOnLoad keeps it alive across scenes.
    public static DialogDisplay Instance =>
        UIController.Instance != null ? UIController.Instance.Get<DialogDisplay>() : null;

    public override UILayer Layer => UILayer.Modal;

    // Only reaches the dialog while it's the top screen, so a prompt on top of it
    // takes the input instead of advancing the text.
    public override bool HandleSubmit()
    {
        inputReceived = true;
        return true;
    }

    public override bool HandleCancel()
    {
        inputReceived = true;
        return true;
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
        DisplayLines(new[] { systemMessage });
    }

    // Shows lines in order, waiting for input after each, then closes the box.
    // If dialog is already showing, they're added after the current lines.
    public void DisplayLines(IEnumerable<DialogLineData> newLines)
    {
        List<DialogLineData> toAdd = new List<DialogLineData>(newLines);
        if (toAdd.Count == 0) return;

        if (running == null)
        {
            // Open before queueing. Opening activates this object so the coroutine can start,
            // and finishes any close animation from the last conversation, whose OnClose
            // would otherwise clear the lines being added here.
            // Does nothing if the box is already open (e.g. held open by ShowPrompt).
            UIController.Instance.Open<DialogDisplay>();
        }

        foreach (DialogLineData line in toAdd) lines.Enqueue(line);

        if (running == null) running = StartCoroutine(DisplayMessagePaginated());
    }

    // Types a system message and leaves the box open without waiting for input,
    // so a ChoicePrompt can appear under the question right away. Yield on this, then either
    // call DisplayLines / DisplaySystemMessage to continue in the same box, or Hide() to close it.
    public IEnumerator ShowPrompt(string message)
    {
        // Let any dialog that's already playing finish first.
        while (running != null) yield return null;

        UIController.Instance.Open<DialogDisplay>();

        DialogLineData line = new DialogLineData();
        line.speakerID = -1;
        line.dialogText = message;

        running = StartCoroutine(TypeLine(line));
        yield return running;
        running = null;

        // Lines queued while the prompt was typing would otherwise never start.
        if (lines.Count > 0) running = StartCoroutine(DisplayMessagePaginated());
    }

    // Closes the box, e.g. after a prompt is declined.
    public void Hide()
    {
        if (!IsOpen) return;
        EndConversation();
    }

    protected override void OnOpen()
    {
        OnMessageStart?.Invoke();
    }

    protected override void OnClose()
    {
        // Covers the dialog being closed by something else mid-conversation.
        if (running != null) StopCoroutine(running);
        running = null;
        lines.Clear();
    }

    private IEnumerator DisplayMessagePaginated()
    {
        while (lines.Count > 0)
        {
            yield return TypeLine(lines.Dequeue());

            inputReceived = false;
            yield return new WaitUntil(() => inputReceived);

            yield return null;
        }

        EndConversation();
    }

    // Types one line, waiting for input between pages. Doesn't wait at the end.
    private IEnumerator TypeLine(DialogLineData currentMessage)
    {
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
    }

    private void EndConversation()
    {
        running = null;
        UIController.Instance.Close(this);
        OnMessageEnd?.Invoke();
    }

    private void HandleMessageType(int speakerID)
    {
        if (speakerID >= 0)
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