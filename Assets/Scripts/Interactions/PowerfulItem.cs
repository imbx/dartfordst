using UnityEngine;
using UnityEngine.Events;
using BoxScripts;
public class PowerfulItem : Item {
    public UnityEvent Action;

    public override void Execute(bool isLeftAction = true) {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        Debug.Log("[PowerfulItem] Executing");
        isInteractingThis = true;
        if(isLeftAction && gameControllerObject.state != GameState.LOOKITEM) {
            if(startTransform == null) 
            {
                gameObject.layer = 8;
                tag = "Item";
                startTransform = new TransformData(transform);
                movement.SetConfig(2f, true);
                movement.SetParameters(new TransformData(GameController.current.Hand.position, Vector3.zero), startTransform);
            } else {
                gameObject.layer = 6;
                movement.Invert();
                tag = MainTag;
            }
            gameControllerObject.ChangeState(GameState.LOOKITEM);
        }
        
        if(movement.isAtDestination) {
            Debug.Log("[PowerfulItem] Is at destination, execute");
            if(isLeftAction && CanPickup)
            {
                Debug.Log("[PowerfulItem] Picking up");
                Action.Invoke();
                OnEnd();
                return;
            }
            this.isLeftAction = isLeftAction;
        }
    }
    
}