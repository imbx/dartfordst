using UnityEngine;
using UnityEngine.Events;

public class InteractOnAction : InteractBase {

    public UnityEvent<InteractOnAction> OnAction;

    public override void Execute(bool isLeftAction = true)
    {
        if(!isLeftAction) OnAction.Invoke(this);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}