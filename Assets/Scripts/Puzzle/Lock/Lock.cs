using System.Collections.Generic;
using UnityEngine;

public class Lock : PuzzleBase {

    public List<LockWheel> keys;

    public string FinalCombination = "123";

    [FMODUnity.EventRef]
    public string candadoSound = "event:/candado2d";

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        if (isInteractingThis && GetCurrentCombination().Equals(FinalCombination))
        {
            OnEnd(true);
            GameController.current.music.playMusic(candadoSound);
        }
        
    }
    protected override void OnEnd(bool destroyGameObject = false)
    {
        foreach(LockWheel lw in keys)
        {
            lw.DestroyInteraction();
        }
        base.OnEnd(destroyGameObject);
    }

    private string GetCurrentCombination()
    {
        string combination = "";

        foreach(LockWheel lcc in keys)
        {
            combination += lcc.Number.ToString();
            
        }
        Debug.Log("[Lock] Current combination is " + combination);
        return combination;
    }
    
}