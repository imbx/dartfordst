using UnityEngine;

public class Cube : InteractBase {
    private Vector3 InitialPos; 

    protected override void OnStart() {

        GameController current = GameController.current;

        // mover objeto al punto pos

        if(current.database.GetProgressionState(1)){

        }
    }

    public override void Execute(bool isLeftAction = true) {
        OnStart();
    }

    
}