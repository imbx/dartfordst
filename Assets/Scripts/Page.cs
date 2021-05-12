using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour {
    private Text content;
    private void OnEnable() {
        content = GetComponent<Text>();
    }

    public bool FillInformation(int id, bool isDiary) {
        string text = 
                isDiary ? 
                GameController.current.database.GetDiaryPage(id):
                GameController.current.database.GetNotesPage(id);
        if(text == null) return false;
        else content.text = text;
        return true;
    }

    public void SetText(string txt)
    {
        content.text = txt;
    }
}