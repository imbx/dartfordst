using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SafeBoxChecker : InteractBase {
    [Header("Safe Box Checker Parameters")]
    public UnityEvent Action;
    private bool isOpening = false;

    public override void Execute(bool isLeftAction = true)
    {
        if(isOpening) return;
        base.Execute(isLeftAction);
        isOpening = true;
        StartCoroutine(Open());
    }
    

    IEnumerator Open()
    {

        Action.Invoke();
        yield return null;
    }
}