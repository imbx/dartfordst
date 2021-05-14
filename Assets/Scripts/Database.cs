using System;
using System.Collections;
using System.Collections.Generic;
using BoxScripts;
using UnityEngine;

public class Database {
    private Dictionary<int, string>  Dialogues;
    private Dictionary<int, NotebookPage>  Diary;
    private Dictionary<int, NotebookPage>  Notes;
    private Dictionary<int, bool> PlayerProgression;

    public Database() {
        Dialogues = new Dictionary<int, string>();
        PlayerProgression = new Dictionary<int, bool>();
        Diary = new Dictionary<int, NotebookPage>();
        Notes = new Dictionary<int, NotebookPage>();
        LoadData();
    }

    private bool LoadData() {
        //Dialogues.Add(1, "Test dialogue");
        ParseDialogueData("Database/dialogues");
        PlayerProgression.Add(-1, true);
        PlayerProgression.Add(1, true);


        Diary.Add(78325, new NotebookPage("test page 1"));
        Diary.Add(324234, new NotebookPage("test page 2"));
        Diary.Add(2342342, new NotebookPage("test page 3"));
        Diary.Add(232341, new NotebookPage("test page 4"));
        Diary.Add(646533, new NotebookPage("test page 5"));
        
        //Notes.Add(5223, new NotebookPage("La llave esta en el jarron"));
        

        PlayerProgression.Add(78325, true);
        PlayerProgression.Add(324234, true);
        PlayerProgression.Add(2342342, true);
        PlayerProgression.Add(232341, true);
        PlayerProgression.Add(646533, true);
        //PlayerProgression.Add(5223, true);
        PlayerProgression.Add(3652, true);
        PlayerProgression.Add(1234, true);
        PlayerProgression.Add(6434, true);
        PlayerProgression.Add(3234, true);


        //PlayerProgression.Add(1242, true);
        //PlayerProgression.Add(1243, true);
        //PlayerProgression.Add(126, true);
        //PlayerProgression.Add(12432, true);

        AddNotePage(5223, "La llave esta en el jarron");

        Notes.Add(12432, new NotebookPage("Paris"));
        Notes.Add(1242,  new NotebookPage("... 1 \n -.. 2 \n --. 3 \n --- 4 \n .-- 5 \n ..- 6 \n .-. 7"));
        Notes.Add(126, new NotebookPage("La llave esta en la estanteria de la cocina"));
        Notes.Add(1243,  new NotebookPage("...-..--."));






        return true;
    }


    public void AddNotePage (int identifier, string page)
    {
        AddProgressionID(identifier, true);
        Notes.Add(identifier, new NotebookPage(page));

        Debug.Log("[Database] Note " + identifier + " added : " + page);
    }

    public void ParseDialogueData(string fileName)
    {
        var textAsset = Resources.Load<TextAsset>(fileName);
        var splitDataset = textAsset.text.Split(new char[] {'\n'});

        for(int i = 0; i < splitDataset.Length; i++)
        {
            string[] row = splitDataset[i].Split(new char[] {';'});
            if(row.Length == 2)
            {
                Dialogues.Add(int.Parse(row[0]), row[1]);
                Debug.Log("[Database] Dialogue added " + row[0] + " : " + row[1]);
            }
        }
    }

    public bool EditProgression(int eventID, bool check = true) {
        if(ProgressionExists(eventID)){
            Debug.Log("[Edit Progression] " + eventID + " is now " + check);
            PlayerProgression[eventID] = check;
            return true;
        }
        return false;
    }
    public void AddProgressionID(int _id, bool state = false) {
        if(ProgressionExists(_id)) return;
        PlayerProgression.Add(_id, state);
    }

    public bool ProgressionExists(int eventID) {
        return PlayerProgression.ContainsKey(eventID);
    }

    public bool GetProgressionState(int eventID)
    {
        return ProgressionExists(eventID) ? PlayerProgression[eventID] : false;
    }

    public string GetDialogue(int dialogueID)
    {
        return Dialogues.ContainsKey(dialogueID) ? Dialogues[dialogueID] : null;
    }

    public string GetDiaryPage (int diaryID)
    {
         return GetProgressionState(diaryID) ? Diary[diaryID].text : null;
    }

    public string GetNotesPage (int noteID)
    {
         return GetProgressionState(noteID) ? Notes[noteID].text : null;
    }
}