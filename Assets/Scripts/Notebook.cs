using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxScripts;

public class Notebook : MonoBehaviour
{
    private int[] notebookPosition;
    private NotebookType notebookType;
    [SerializeField] private List<int> diaryIdentifier;
    [SerializeField] private List<int> notesIdentifier;
    // [SerializeField] private List<int>[] notebookIdentifier;
    public List<DiaryController> dController;
    [SerializeField] private Page leftPage;
    [SerializeField] private Page rightPage;
    // Start is called before the first frame update

    public void CreateNotebook(int numberOfOptions = 2) { 
        notebookPosition = new int[numberOfOptions];
        
        if(diaryIdentifier.Count <= 0) 
        {
            diaryIdentifier = new List<int>();

            // HERE SHOULD FILL IDENTIFIERS IN CASE THERE ARE NONE SET IN GAMEOBJECT
        }
        if(notesIdentifier.Count <= 0) 
        {
            notesIdentifier = new List<int>();

            // HERE SHOULD FILL IDENTIFIERS IN CASE THERE ARE NONE SET IN GAMEOBJECT
        }
        
        // notebookStrings = new List<int>();

        for(int i = 0; i < numberOfOptions; i++) notebookPosition[i] = 0;

        Debug.Log("-- NOTEBOOK LOADED --");
    }
    private void Awake() {
        CreateNotebook();
    }

    private void OnEnable() {
        Debug.Log("Creating notebook");
        CreateNotebook();
        LoadData();
        // GameController.current.ChangeState(GameState.OPENNOTEBOOK);
    }

    private void LoadData()
    {
        notebookType = NotebookType.Diary;
        Debug.Log("Setting pages");
        SetPages();
    }

    public void OnClickNext()
    {
        Debug.Log("Next");
        if(notebookPosition[(int) notebookType] < GetPagesCount() - 2)
            notebookPosition[(int) notebookType] += 2;
        SetPages();
    }

    public void OnClickPrev()
    {
        Debug.Log("Prev");
        if(notebookPosition[(int) notebookType] >= 2)
            notebookPosition[(int) notebookType] -= 2;
        SetPages();
    }

    public void OnChangeCategory(int i) 
    {
        Debug.Log("Want to change category  to " + (NotebookType) i);
        if(notebookType != (NotebookType) i) {
            notebookType = (NotebookType) i;
            SetPages();
            Debug.Log("Cat changed");
        }
    }
    void SetPages()
    {
        int nPosition = (int) notebookType;
        Debug.Log("Left page identifier: " + GetPageIdentifier(notebookPosition[(int) notebookType]));
        // Debug.Log("Left page text: " + GameController.current.database.GetDiaryPage(GetPageIdentifier(notebookPosition[(int) notebookType])));
        leftPage.FillInformation(
            GetPageIdentifier(notebookPosition[(int) notebookType]),
            notebookType == NotebookType.Diary ? true : false
            );
        
        int maxPages = notebookType == NotebookType.Diary ? diaryIdentifier.Count : notesIdentifier.Count; 
        if(notebookPosition[(int) notebookType] < maxPages  - 1){
            Debug.Log("Right page identifier: " + GetPageIdentifier(notebookPosition[(int) notebookType] + 1));
            // Debug.Log("Right page text: " + GameController.current.database.GetDiaryPage(GetPageIdentifier(notebookPosition[(int) notebookType] + 1)));
            rightPage.FillInformation(
                GetPageIdentifier(notebookPosition[(int) notebookType] + 1),
                notebookType == NotebookType.Diary ? true : false
                );
        } 
        else rightPage.SetText("");
        Debug.Log("-- PAGES SET --");
    }

    private int GetPagesCount()
    {
       switch(notebookType)
        {
            case NotebookType.Diary:
                return diaryIdentifier.Count;
            case NotebookType.Notes:
                return notesIdentifier.Count;
            default:
                return -1;
        }
    }

    private int GetPageIdentifier(int nPos)
    {
        switch(notebookType)
        {
            case NotebookType.Diary:
                return diaryIdentifier[nPos];
            case NotebookType.Notes:
                return notesIdentifier[nPos];
            default:
                return -1;
        }
    }
}
