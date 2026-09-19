using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogParser dialogDatabase;
    public static DialogController instance;
    public DialogBox dialogBox;
    [ShowInInspector] Dictionary<int, StorySequenceData> storySequences = new();
    [ShowInInspector] Dictionary<int, DialogLineData> dialogLines = new();

    private Queue<DialogLineData> dialogQueue = new Queue<DialogLineData>();

    private bool lineComplete = false;
    private int nextLine = -1;
    public static Action onDialogStarted;
    public static Action onDialogCompleted;

    private FlagManager flagManager;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Initialize();
    }
    [ContextMenu("Initialize")]
    private void Initialize()
    {
        DialogBox.onMessageComplete += DialogCompleted;
        DialogBox.onMessageStart += DialogStarted;
        flagManager = FlagManager.Instance;
        dialogDatabase.ParseData();
        storySequences = dialogDatabase.GetStorySequences();
        dialogLines = dialogDatabase.GetDialogLines();
        //StartStorySequence(0);

    }

    private void DialogCompleted() { 
        lineComplete = true;
        onDialogCompleted?.Invoke();
    }
    private void DialogStarted()
    {
        onDialogStarted?.Invoke();
    }
 

    public int DisplayDialogLine(int id)
    {
        DialogLineData dialog = dialogLines[id];
        int lineID = dialog.dialogID;
        if (dialog.conditionID != "")
        {
            if (flagManager.CheckFlag(dialog.conditionID))
            {
                lineID = dialog.dialogID;
            }
            else
            {
                lineID = dialog.fallbackLine;
            }
        }

        
        if(dialog.triggerID != "")
        {
            flagManager.SetFlag(dialog.triggerID, true);
        }

        dialogBox.ShowMessage(dialogLines[lineID]);
        
        return nextLine;

    }

    public void DisplayDialogLines(List<int> indexes)
    {
        Queue<DialogLineData> dialogQueue = new Queue<DialogLineData>();
        foreach(var i in indexes)
        {
            dialogQueue.Enqueue(dialogLines[i]);
        }
        dialogBox.ShowMessage(dialogQueue);

    }



    public void UpdateNextLine(int nextLine_)
    {
        nextLine = nextLine_;
    }
    public void StartStorySequence(int id_)
    {
        //StartCoroutine(PlayStorySequence(id_));
        UpdateDialogQueue(id_, dialogQueue);
        dialogBox.ShowMessage(dialogQueue);
      //  StartCoroutine(PlayStorySequence(dialogQueue));
    }

    public void UpdateDialogQueue(int startingID_, Queue<DialogLineData> dialogQueue_)
    {
        dialogQueue_.Enqueue(dialogLines[startingID_]);
        if(dialogLines[startingID_].nextLine != -1)
        {
            UpdateDialogQueue(dialogLines[startingID_].nextLine, dialogQueue_);
        }
    }

    private IEnumerator PlayStorySequence(Queue<DialogLineData> dialogQueue_)
    {
        bool playingScene = true;
        nextLine = dialogQueue_.Dequeue().dialogID;
        while (playingScene)
        {

            DisplayDialogLine(nextLine);
            lineComplete = false;
            yield return new WaitUntil(() => lineComplete == true);
            if (dialogLines[nextLine].nextLine == -1)
            {
                playingScene = false;
            }
            nextLine = dialogQueue_.Dequeue().dialogID;
            yield return null;
        }
        yield return null;
    }


}
