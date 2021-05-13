using UnityEngine;
using System.Collections.Generic;

public class LinkNotes : PuzzleBase {
    public List<Note> notes;

    private bool hasGivenPage = false;

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
        if(notes.Count <= 0 && !hasGivenPage) {
            hasGivenPage = true;
            GameController.current.database.AddNotePage(126, "La llave esta en la estanteria de la cocina");
            this.OnEnd();
        }
    }
}