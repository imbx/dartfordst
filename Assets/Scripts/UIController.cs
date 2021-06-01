using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public GameObject Notebook;
    public DiaryController notebookController;
    public GameObject BasicInteract;
    public GameObject PicturesInteract;

    public List<GameObject> TargetUIObjects;

    public GameObject RadioText;

    public void SetNotebookActive(bool active = true) {
        Notebook.SetActive(active);
        //Notebook.GetComponent<Notebook>().enabled = active;
    }

    public void HideUI()
    {
        Debug.Log("Hiding UI");
        SetBasicInteract(false);
        SetPicturesInteract(false);
    }

    public void SetBasicInteract(bool active = true)
    {
        Debug.Log("(Un)Showing Basic Interactions UI");
        BasicInteract.SetActive(active);
    }

    public void SetPicturesInteract(bool active = true)
    {
        Debug.Log("(Un)Showing Pictures Interactions UI");
        PicturesInteract.SetActive(active);
    }


    public void TargetUI(string targetTag)
    {

        if(targetTag == "RadioOn")
        {
            RadioText.SetActive(true);
        } else RadioText.SetActive(false);
        
        foreach(GameObject gObject in TargetUIObjects)
        {
            if(gObject.activeInHierarchy && gObject.name != targetTag) gObject.SetActive(false);
            else if(gObject.name == targetTag) gObject.SetActive(true);
        }
    }

    public void ForceDiaryPage(int reqId)
    {
        notebookController.forcedReqId = reqId;
        notebookController.wantToForcePage = true;
    }
}