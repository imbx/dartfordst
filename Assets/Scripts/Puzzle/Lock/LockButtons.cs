using UnityEngine;
using UnityEngine.Events;

public class LockButtons : InteractBase {

    public UnityEvent<bool> OnAction;
    public bool isPositive = true;

    public override void Execute(bool isLeftAction = true)
    {
        OnAction.Invoke(isPositive);
    }
    
}