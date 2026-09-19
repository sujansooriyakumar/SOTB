using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Sirenix.Serialization;
public class DialogParser:MonoBehaviour
{
    public string filePath;
    public CharacterDB characterDB;
    public Dictionary<int, StorySequenceData> storySequences = new();
    public Dictionary<int, DialogLineData> dialogLines = new();


    #region

    private static int SPEAKER_ID_COLUMN = 0;
    private static int SPEAKER_NAME_COLUMN = 1;
    private static int DIALOG_ID_COLUMN = 2;
    private static int DIALOG_LINE_COLUMN = 3;
    private static int CONDITION_KEY_COLUMN = 4;
    private static int TRIGGER_KEY_COLUMN = 5;
    private static int NEXT_LINE_COLUMN = 6;
    private static int FALLBACK_LINE_COLUMN= 7;

    #endregion
    public void ParseData()
    {
        string[] lines = System.IO.File.ReadAllLines(
            filePath,
            System.Text.Encoding.UTF8
        );

        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = Regex.Split(
                lines[i],
                ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"
            );


            int speakerID = GetInt(columns, SPEAKER_ID_COLUMN);
            string speakerName = GetString(columns, SPEAKER_NAME_COLUMN);
            int dialogID = GetInt(columns, DIALOG_ID_COLUMN);
            string dialogLine = GetString(columns, DIALOG_LINE_COLUMN);
            string conditionKey = GetString(columns, CONDITION_KEY_COLUMN);
            string triggerKey = GetString(columns, TRIGGER_KEY_COLUMN);
            int nextLine = GetInt(columns, NEXT_LINE_COLUMN);
            int fallbackLine = GetInt(columns, FALLBACK_LINE_COLUMN);
            if(conditionKey != "") FlagManager.Instance.AddFlag(conditionKey);
            // Skip completely invalid rows
            if (dialogID == -1)
                continue;

            if (speakerID == -1)
                continue;

            DialogLineData lineData = new DialogLineData(
                dialogLine,
                characterDB.GetCharacterData(speakerID),
                conditionKey,
                triggerKey,
                nextLine,
                fallbackLine,
                dialogID
            );

            dialogLines[dialogID] = lineData;

           
        }
    }

    private int GetInt(string[] columns, int index, int defaultValue = -1)
    {
        if (index >= columns.Length)
            return defaultValue;

        string value = Clean(columns[index]);

        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    private string GetString(string[] columns, int index, string defaultValue = "")
    {
        if (index >= columns.Length)
            return defaultValue;

        string value = Clean(columns[index]);

        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        // Normalize unicode ellipsis to three dots
        value = value.Replace("…", "...");



        return value;
    }


    private string Clean(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        value = value.Trim().Trim('"');

        // Fix common encoding fallout
        value = value.Replace("�", "..."); // fallback
        value = value.Replace("…", "..."); // normalize ellipsis

        return value;
    }


    private bool IsOnlyEllipses(string value)
    {
        // Remove whitespace and dots, check if anything meaningful remains
        foreach (char c in value)
        {
            if (c != '.' && !char.IsWhiteSpace(c))
                return false;
        }

        return true;
    }





    public Dictionary<int, StorySequenceData> GetStorySequences() { return storySequences; }
    public Dictionary<int, DialogLineData> GetDialogLines() { return dialogLines; }
}
