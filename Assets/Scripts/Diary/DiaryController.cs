using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxScripts;

public class DiaryController : MonoBehaviour
{
    private int currentPage;
    public List<DiaryPage> Pages;

    private List<int> DiaryPositions;
    private List<int> NotesPosition;
    private int MaxList = 0;

    [SerializeField] private DiaryNodes diaryNodes;

    [HideInInspector] public bool wantToForcePage = false;
    [HideInInspector] public int forcedReqId = 0;


    
    private void Awake() {
        DiaryPositions = new List<int>();
        NotesPosition = new List<int>();
    }

    void OnEnable()
    {
        SearchPages();
    }

    private void SearchPages()
    {
        int counter = 0;
        for(int i = 0 ; i < Pages.Count; i++)
        {
            DiaryPage diaryPage = Pages[i];
            diaryPage.SetActive(false);
            Debug.Log("[DiaryController] Page REQID : " + diaryPage.ReqID + " has state " + GameController.current.database.GetProgressionState(diaryPage.ReqID));
            if(GameController.current.database.GetProgressionState(diaryPage.ReqID)){
                counter++;
                switch(diaryPage.pageType)
                {
                    case NotebookType.Diary:
                        AddToDiary(i);
                        break;
                    case NotebookType.Notes:
                        AddToNotes(i);
                        break;
                    default:
                        break;
                }
            }
        }
        MaxList = counter;
        diaryNodes.ResetLists();
        diaryNodes.SetDiaryNodes(DiaryPositions.Count);
        diaryNodes.SetNotesNodes(NotesPosition.Count);
        if(wantToForcePage)
        {
            wantToForcePage = false;
            ForcePage(forcedReqId);
        }
        else SetNewPage(0);
    }

    private void AddToDiary (int newId)
    {
        if(DiaryPositions.Contains(newId)) return;
        DiaryPositions.Add(newId);
    }
    private void AddToNotes (int newId)
    {
        if(NotesPosition.Contains(newId)) return;
        NotesPosition.Add(newId);
    }

    public void SetNewPage(int pageId)
    {
        if(pageId < MaxList)
        {
            Pages[GetPageFromListPost(currentPage)].SetActive(false);
            Pages[GetPageFromListPost(pageId)].SetActive(true);
            diaryNodes.SelectCircle(currentPage, pageId);
            currentPage = pageId;
        }
    }

    public void ForcePage(int pageReq)
    {
        int pageCounter = 0;

        foreach(int i in DiaryPositions)
        {
            if(Pages[i].ReqID == pageReq) {
                SetNewPage(pageCounter);
                return;
            }
            pageCounter++;
        }

        foreach(int i in NotesPosition)
        {
            if(Pages[i].ReqID == pageReq) {
                SetNewPage(pageCounter);
                return;
            }
            pageCounter++;
        }
    }

    private int GetPageFromListPost(int listPos)
    {
        int pageId = 0;
        Debug.Log("[DiaryController] Want to change to page " + listPos);
        if(listPos > DiaryPositions.Count - 1)
        {
            pageId = NotesPosition[listPos - DiaryPositions.Count];
        } else pageId = DiaryPositions[listPos];
        return pageId;
    }

    public void OnClickNext()
    {
        Debug.Log("[DiaryController] Next");
        int page = currentPage;
        if(page < MaxList)
            page++;
        else return;
        SetNewPage(page);
    }

    public void OnClickPrev()
    {
        Debug.Log("[DiaryController] Prev");
        int page = currentPage;
        if(page > 0)
            page--;
        else return;
        SetNewPage(page);
    }

}
