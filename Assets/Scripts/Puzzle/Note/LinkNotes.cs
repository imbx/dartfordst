using UnityEngine;
using System.Collections.Generic;

public class LinkNotes : PuzzleBase {
    public List<Note> notes;
    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    public void RemoveNote(Note nt)
    {
        notes.Remove(nt);
        Destroy(nt);
    }

    private void Update() {
        if(!isInteractingThis) return;
        if(notes.Count <= 0) this.OnEnd();
        if(controller.isEscapePressed)
        {
            Debug.Log("[LinkNotes] Escape pressed");
            this.OnExit();
        }
    }
}