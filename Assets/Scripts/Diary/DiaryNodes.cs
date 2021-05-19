using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiaryNodes : MonoBehaviour {
    private int diaryNodesCount = 0;
    private float diaryNodesWidth = 0;
    private int notesNodesCount = 0;
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
    [SerializeField] private List<GameObject> diaryNodes;
    [SerializeField] private List<GameObject> notesNodes;

    public UnityEvent<int> OnDotEvent;

    public void ResetLists()
    {
        DestroyNodes();
    }

    public void SetDiaryNodes(int diaryNodes)
    {
        // if(diaryNodesCount == diaryNodes) return;
        diaryNodesCount = diaryNodes;
        SetNodes(true);
    }

    public void SetNotesNodes(int notesNodes)
    {
        // if(notesNodesCount == notesNodes) return;
        notesNodesCount = notesNodes;
        SetNodes(false);
    }

    private void DestroyNodes()
    {   if(diaryNodes != null) foreach(GameObject diaryObject in diaryNodes) Destroy(diaryObject);
        diaryNodes = new List<GameObject>();
        if(notesNodes != null) foreach(GameObject noteObject in notesNodes) Destroy(noteObject);
        notesNodes = new List<GameObject>();
    }

    private void SetNodes(bool isDiary)
    {
        int nodesLeft = 0;
        if(isDiary)
            nodesLeft = Mathf.Abs(diaryNodes.Count - diaryNodesCount);
        else
            nodesLeft = Mathf.Abs(notesNodes.Count - notesNodesCount);

        for(int i = 0; i < nodesLeft; i++)
        {
            bool isFirst = false;
            GameObject goParent = Instantiate(nodePrefab);
            //new GameObject("Node " + diaryNodesCount + i, typeof(RectTransform));
            //goParent.AddComponent<DiaryPoint>();
            
            goParent.transform.SetParent(transform);
            if((isDiary && diaryNodes.Count == 0 && i == 0) || (!isDiary && notesNodes.Count == 0  && i == 0)) isFirst = true;

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
            goParent.GetComponent<DiaryPoint>().SetData(isDiary ? diaryNodes.Count : diaryNodes.Count + notesNodes.Count, this);
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

            if(notesNodesCount == 0) separator.SetActive(false);
            else separator.SetActive(true);

            if(isDiary && i == nodesLeft - 1)
            {
                separator.GetComponent<RectTransform>().anchoredPosition = new Vector2(diaryNodesWidth + (separatorWidth * 0.5f) - (NodeSpan * 0.5f), 0f);
            }

            if(isDiary) diaryNodes.Add(goParent);
            else notesNodes.Add(goParent);
        }
        Debug.Log("[DiaryNodes] dNodeWidth : " + diaryNodesWidth + " notesNodesWidth : " + notesNodesWidth);
        float totalWidth = 0.5f * (diaryNodesWidth + (notesNodes.Count > 0 ? separatorWidth + notesNodesWidth : 0));
                Debug.Log("[DiaryNodes] totalWidth : " + totalWidth);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-totalWidth + NodeSpan, 96f);
        leftArrow.anchoredPosition = new Vector2(-totalWidth - 48f , 96f);
        rightArrow.anchoredPosition = new Vector2(totalWidth + (48f * 2f), 96f);
    }

    public void SetPointAction(int newId)
    {
        OnDotEvent.Invoke(newId);
    }

    public void SelectCircle(int prevPoint, int newPoint)
    {
        if(prevPoint > diaryNodesCount - 1)
        {
            prevPoint -= diaryNodesCount;
            notesNodes[prevPoint].GetComponent<DiaryPoint>().DotImage.sprite = EmptyCircle;
        } else diaryNodes[prevPoint].GetComponent<DiaryPoint>().DotImage.sprite = EmptyCircle;

        if(newPoint > diaryNodesCount - 1)
        {
            newPoint -= diaryNodesCount;
            notesNodes[newPoint].GetComponent<DiaryPoint>().DotImage.sprite = FilledCircle;
        } else diaryNodes[newPoint].GetComponent<DiaryPoint>().DotImage.sprite = FilledCircle; 
    }
}