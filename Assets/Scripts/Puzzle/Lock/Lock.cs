using System.Collections.Generic;
using UnityEngine;

public class Lock : PuzzleBase {

    public List<LockWheel> keys;

    public string FinalCombination = "123";

    [FMODUnity.EventRef]
    public string candadoSound = "event:/candado";
    FMOD.Studio.EventInstance lockEvent;


    private void Start()
    {
        lockEvent = FMODUnity.RuntimeManager.CreateInstance(candadoSound);
    }


    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        if (GetCurrentCombination().Equals(FinalCombination))
        { 
            this.OnEnd(true);
            lockEvent.start();
        
        }

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