using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BoxScripts;
using UnityEngine;

public class Database {
    private Dictionary<int, string>  Dialogues;
    private Dictionary<int, bool> PlayerProgression;
    public EntityData playerData;

    public Database(EntityData entityData) {
        Dialogues = new Dictionary<int, string>();
        PlayerProgression = new Dictionary<int, bool>();
        playerData = entityData;
        // Try to load
        LoadGame();
        LoadData();
    }

    private bool LoadData() {
        //Dialogues.Add(1, "Test dialogue");
        ParseDialogueData("Database/dialogues");
        PlayerProgression.Add(-1, true);
        PlayerProgression.Add(0, true);
        PlayerProgression.Add(1, true);

        
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
        return true;
    }

    
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.PlayerProgression = PlayerProgression;
        save.playerPosition = playerData.PlayerPosition;
        save.cameraRotation = playerData.CameraRotations;
        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("[Database] Game saved");
    }


    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            playerData.isLoadingData = true;

            PlayerProgression = save.PlayerProgression;
            playerData.PlayerPosition = save.playerPosition;
            playerData.CameraRotations = save.cameraRotation;

            Debug.Log("[Database] Game loaded");

        } else playerData.isLoadingData = false;
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
}
[System.Serializable]
public class Save
{
    public Dictionary<int, bool> PlayerProgression;
    public Vector3 playerPosition;
    public Vector3 cameraRotation;
}