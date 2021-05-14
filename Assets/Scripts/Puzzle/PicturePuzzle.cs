using System;
using System.Collections.Generic;
using UnityEngine;

public class PicturePuzzle : MonoBehaviour {
    public int Identifier = 1242;
    public List<Picture> pictures;
    private Picture first;
    private bool hasGivenPage = false;

    private void OnEnable() {
        if(GameController.current.database.ProgressionExists(Identifier)){
            Debug.Log("Deactivating " + name);
        }
        else GameController.current.database.AddProgressionID(Identifier);
    }
    void Update()
    {
        if(CheckPictures())
        {
            hasGivenPage = true;
            GameController.current.database.EditProgression(Identifier);
            Debug.Log("[PicturePuzzle] Ended");
        }
    }

    public bool CheckPictures()
    {
        int lastPicture = -1;
        foreach(Picture p in pictures)
        {
            if(p.CheckRotation()){
                if(lastPicture == -1 || lastPicture + 1 == p.Identifier)
                    lastPicture = p.Identifier;
                else return false;
            } else return false;
            
        }
        return true;
    }

    public void SwapPicture(Picture pic)
    {
        if(first)
        {
            int identifier = GetPictureId(first);
            int secIdentifier = GetPictureId(pic);
            Picture p = pictures[identifier];
            pictures[identifier] = pic;
            pictures[secIdentifier] = p;
            first = null;
        }
        else first = pic;
    }

    private int GetPictureId(Picture pic)
    {
        if(pictures.Count > 0)
            for(int i = 0; i < pictures.Count; i++)
            {
                if(pictures[i].Identifier == pic.Identifier) return i;
            }
        return 0;
    }
}