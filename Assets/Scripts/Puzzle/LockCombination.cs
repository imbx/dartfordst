using System.Collections.Generic;
using UnityEngine;

public class LockCombination : PuzzleBase {

    public List<LockCombinationController> keys;

    public string FinalCombination = "123";

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        this.OnStart();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        if(GetCurrentCombination().Equals(FinalCombination)) Debug.Log("GOT IT");
    }

    private string GetCurrentCombination()
    {
        string combination = "";

        foreach(LockCombinationController lcc in keys)
        {
            combination += lcc.Number.ToString();
        }
        return combination;
    }
}