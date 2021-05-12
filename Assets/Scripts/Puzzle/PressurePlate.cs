using UnityEngine;

public class PressurePlate : InteractBase {

    public int reqId = 9000;
    public override void Execute(bool isLeftAction = true)
    {
        if(GameController.current.database.GetProgressionState(reqId))
        {
            // Place Item
        }
    }
}