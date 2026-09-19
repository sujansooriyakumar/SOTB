using System.Collections.Generic;
using UnityEngine;

public class DialogLineData
{
    public string dialogLine;
    public CharacterData speaker;
    public string conditionID;
    public string triggerID;
    public string conditionFlag;
    public int nextLine;
    public int fallbackLine;
    public int dialogID;


    public DialogLineData(string dialogLine_, CharacterData characterData_, string conditionID_, string triggerID_, 
        int nextLine_, int fallbackLine_, int dialogID_)
    {
        this.dialogLine = dialogLine_;
        this.speaker = characterData_;
        this.conditionID = conditionID_;
        this.triggerID = triggerID_;
        this.nextLine = nextLine_;
        this.fallbackLine = fallbackLine_;
        this.dialogID = dialogID_;
    }
}
