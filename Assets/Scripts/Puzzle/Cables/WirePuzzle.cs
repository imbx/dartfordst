using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePuzzle : PuzzleBase
{
    [Header("Wire Puzzle Parameters")]
    public GameObject Tapa;
    [SerializeField] private List<Knot> tornillos;

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        if(Tapa) Tapa.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update() {
        if(isInteractingThis && controller.isEscapePressed)
        {
            Debug.Log("[PuzzleBase] Called Escape");
            this.OnEnd();
        }
    }


}
