using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using BoxScripts;

public class MapPuzzle : PuzzleBase {

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        this.OnStart();
        GetComponent<BoxCollider>().enabled = false;
    }
}