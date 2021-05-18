using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DiaryNodes : MonoBehaviour {
    private int dNodesCount = 0;
    private float diaryNodesWidth = 0;
    private int nNodesCount = 0;
    private float notesNodesWidth = 0;

    public float NodeSpan = 44f;

    public GameObject nodePrefab;
    public GameObject pointPrefab;
    public GameObject linkPrefab;

    public RectTransform leftArrow;
    public RectTransform rightArrow;

    public Sprite EmptyCircle;
    public Sprite FilledCircle;


    [SerializeField] private GameObject separator;
    public float separatorWidth = 126f;
    private List<GameObject> dNodes;
    private List<GameObject> nNodes;

    public UnityEvent<int> OnDotEvent;

    private void Awake() {
        dNodes = new List<GameObject>();
        nNodes = new List<GameObject>();
    }

    public void SetDiaryNodes(int diaryNodes)
    {
        if(dNodesCount == diaryNodes) return;
        dNodesCount = diaryNodes;
        SetNodes(true);
    }

    public void SetNotesNodes(int notesNodes)
    {
        if(nNodesCount == notesNodes) return;
        nNodesCount = notesNodes;
        SetNodes(false);
    }

    private void DestroyNodes()
    {
        foreach(GameObject diaryObject in dNodes) Destroy(diaryObject);
        foreach(GameObject noteObject in nNodes) Destroy(noteObject);
    }

    private void SetNodes(bool isDiary)
    {
        if (isDiary) DestroyNodes();
        int nodesLeft = 0;
        if(isDiary)
            nodesLeft = Mathf.Abs(dNodes.Count - dNodesCount);
        else
            nodesLeft = Mathf.Abs(nNodes.Count - nNodesCount);

        for(int i = 0; i < nodesLeft; i++)
        {
            bool isFirst = false;
            GameObject goParent =  Instantiate(nodePrefab);
            //new GameObject("Node " + dNodesCount + i, typeof(RectTransform));
            //goParent.AddComponent<DiaryPoint>();
            
            goParent.transform.SetParent(transform);
            if((isDiary && dNodes.Count == 0 && i == 0) || (!isDiary && nNodes.Count == 0  && i == 0)) isFirst = true;

            if(!isFirst)
            {
                GameObject link = Instantiate(linkPrefab);
                link.transform.SetParent(goParent.transform);
                link.GetComponent<RectTransform>().anchoredPosition = new Vector2(NodeSpan * -0.5f, 0f);
            }

            GameObject point = Instantiate(pointPrefab);
            point.transform.SetParent(goParent.transform);
            point.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            goParent.GetComponent<DiaryPoint>().DotImage = point.GetComponent<Image>();
            goParent.GetComponent<DiaryPoint>().SetData(isDiary ? dNodes.Count : dNodes.Count + nNodes.Count, this);
            goParent.GetComponent<Button>().targetGraphic = goParent.GetComponent<DiaryPoint>().DotImage;
            
            if(isDiary)
            {
                goParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(diaryNodesWidth, 0);
                diaryNodesWidth += NodeSpan;
            }
            else 
            {
                goParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(diaryNodesWidth + separatorWidth + notesNodesWidth, 0);
                notesNodesWidth += NodeSpan;
            }

            if(nNodesCount == 0) separator.SetActive(false);
            else separator.SetActive(true);

            if(isDiary && i == nodesLeft - 1)
            {
                separator.GetComponent<RectTransform>().anchoredPosition = new Vector2(diaryNodesWidth + (separatorWidth * 0.5f) - (NodeSpan * 0.5f), 0f);
            }
            if(isDiary) dNodes.Add(goParent);
            else nNodes.Add(goParent);
        }
        Debug.Log("[DiaryNodes] dNodeWidth : " + diaryNodesWidth + " nNodesWidth : " + notesNodesWidth);
        float totalWidth = 0.5f * (diaryNodesWidth + (nNodes.Count > 0 ? separatorWidth + notesNodesWidth : 0));
                Debug.Log("[DiaryNodes] totalWidth : " + totalWidth);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-totalWidth + NodeSpan, 96f);
        leftArrow.anchoredPosition = new Vector2(-totalWidth - 48f , 96f);
        rightArrow.anchoredPosition = new Vector2(totalWidth + (48f * 2f), 96f);
        dNodesCount = dNodes.Count;
        nNodesCount = nNodes.Count;
    }

    public void SetPointAction(int newId)
    {
        OnDotEvent.Invoke(newId);
    }

    public void SelectCircle(int prevPoint, int newPoint)
    {
        if(prevPoint > dNodesCount - 1)
        {
            prevPoint -= dNodesCount;
            nNodes[prevPoint].GetComponent<DiaryPoint>().DotImage.sprite = EmptyCircle;
        } else dNodes[prevPoint].GetComponent<DiaryPoint>().DotImage.sprite = EmptyCircle;

        if(newPoint > dNodesCount - 1)
        {
            newPoint -= dNodesCount;
            nNodes[newPoint].GetComponent<DiaryPoint>().DotImage.sprite = FilledCircle;
        } else dNodes[newPoint].GetComponent<DiaryPoint>().DotImage.sprite = FilledCircle;
            
    }
}