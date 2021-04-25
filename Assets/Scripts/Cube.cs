using UnityEngine;

public class Cube : InteractBase {
    private Vector3 InitialPos; 

    protected override void OnStart() {

        GameController current = GameController.current;

        // mover objeto al punto pos

        if(current.database.GetProgressionState(1)){
            // grab object
            Vector3 playerPos = current.Player.position;
            // this.transform.position = playerPos;
            transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime);
        }
    }

    public override void Execute() {
        OnStart();
    }

    
}