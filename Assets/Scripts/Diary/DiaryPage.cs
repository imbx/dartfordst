using UnityEngine;
using BoxScripts;
using System.Collections.Generic;

public class DiaryPage : MonoBehaviour {

    public int ReqID = 0;
    public NotebookType pageType;
    public List<GameObject> childs;
    public bool isActive { get { return gameObject.activeInHierarchy; } }

    private void Awake() {
        if(GameController.current)
        {
            if(GameController.current.database.ProgressionExists(ReqID)) return;
            GameController.current.database.AddProgressionID(ReqID);
        }
    }
    public void SetActive(bool setActive)
    {
        gameObject.SetActive(setActive);
    }

    
}