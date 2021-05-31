using UnityEngine;
using BoxScripts;
using UnityEngine.Events;

public class PinSelector : InteractBase {
    public PinSelect pin;
    public UnityEvent<PinSelect> OnAction;

    public override void Execute(bool isLeftAction = true)
    {
        Debug.Log("[PinSelector] Pin set : " + pin);
        OnAction.Invoke(pin);
    }
    
}