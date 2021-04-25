using System;
using System.Collections;
using System.Collections.Generic;

public class Database {
    private Dictionary<int, string>  Dialogues;
    private Dictionary<int, bool> PlayerProgression;
    public Database() {
        Dialogues = new Dictionary<int, string>();
        PlayerProgression = new Dictionary<int, bool>();
    }

    private bool LoadData() {
        Dialogues.Add(1, "Test dialogue");
        PlayerProgression.Add(1, true);
        return true;
    }

    public bool EditProgression(int eventID, bool check = true) {
        if(PlayerProgression.ContainsKey(eventID)){
            PlayerProgression[eventID] = check;
            return true;
        }
        return false;
    }
    public void AddProgressionID(int _id, bool state = false) {
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
}