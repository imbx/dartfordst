using UnityEngine;
using System.Collections.Generic;

public class LinkNotes : PuzzleBase {
    public List<Note> notes;

    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        this.OnStart();
        GetComponent<BoxCollider>().enabled = false;
    }

    public void RemoveNote(Note nt)
    {
        notes.Remove(nt);
        Destroy(nt);
    }

    private void Update() {
        if(notes.Count <= 0) this.OnEnd();
    }
}