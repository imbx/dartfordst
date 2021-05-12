using UnityEngine;

public class UIController : MonoBehaviour {
    public GameObject Notebook;

    public GameObject BasicInteract;
    public GameObject PicturesInteract;

    public void SetNotebookActive(bool active = true) {
        Notebook.SetActive(active);
        Notebook.GetComponent<Notebook>().enabled = active;
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
}