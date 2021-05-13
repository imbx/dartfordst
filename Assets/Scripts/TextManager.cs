using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxScripts;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private TextPosition textPos;
    [SerializeField] private Vector2 Margin = new Vector2(96, 96);

    [Header("Thoughts")]
    [SerializeField] private GameObject ThoughtsContainer;
    [SerializeField] private Text ThoughtsText;
    [SerializeField] private Vector2 ThoughtsLeftTopAnchor = new Vector2(116, 68);
    [SerializeField] private Vector2 ThoughtsRightBottomAnchor = new Vector2(116, 78);

    [Header("Bubble")]
    [SerializeField] private GameObject BubbleContainer;
    [SerializeField] private Text BubbleText;


    private List<int> ThoughtsInQueue;

    private void Awake() {
        ThoughtsInQueue = new List<int> ();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ThoughtsInQueue.Count > 0 && !ThoughtsContainer.activeSelf)
        {
            SpawnThought(ThoughtsInQueue[0]);
            ThoughtsInQueue.Remove(ThoughtsInQueue[0]);
        }
    }

    public void SpawnThought(int dialogueID, TextPosition textPos = TextPosition.BOTTOMLEFT)
    {
        if(!ThoughtsContainer.activeSelf){
            
            string tx = GameController.current.database.GetDialogue(dialogueID);

            Debug.Log("Spawning this text : " + tx);

            float textWidth = CalculateLength(ThoughtsText, tx);
            float textHeight = ThoughtsText.fontSize * 3 * (int)((textWidth + ThoughtsLeftTopAnchor.x + ThoughtsRightBottomAnchor.x + (96f * 2)) / Screen.width);
            textWidth += ThoughtsLeftTopAnchor.x + ThoughtsRightBottomAnchor.x;
            textHeight += ThoughtsLeftTopAnchor.y + ThoughtsRightBottomAnchor.y + 30f;
            ThoughtsText.text = tx;
            ThoughtsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, 224);
            Debug.Log(" W H is " + new Vector2(textWidth, textHeight));
            Debug.Log("Position should be set at : " + new Vector3(-1 * Margin.x, Margin.y, 0));
            ThoughtsContainer.GetComponent<RectTransform>().anchoredPosition = new Vector3(-Margin.x, Margin.y, 0);
            ThoughtsContainer.SetActive(true);
        } else ThoughtsInQueue.Add(dialogueID);
        
    }

    private float CalculateLength(Text textComponent, string message)
    {
        float totalLength = 0;
        Font myFont = textComponent.font;
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = message.ToCharArray();

        foreach(char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, textComponent.fontSize);  
            Debug.Log(totalLength + " + " + characterInfo.advance);
            totalLength += characterInfo.advance + 24f;
        }
        return totalLength;
    }
}
