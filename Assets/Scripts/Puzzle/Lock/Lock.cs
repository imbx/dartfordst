using System.Collections.Generic;
using UnityEngine;

public class Lock : PuzzleBase {

    public List<LockWheel> keys;

    public string FinalCombination = "123";

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        if(GetCurrentCombination().Equals(FinalCombination)) this.OnEnd();
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