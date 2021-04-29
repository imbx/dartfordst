using UnityEngine;

public class Key : InteractBase {
    public override void Execute()
    {
        base.Execute();

        GameController.current.database.AddProgressionID(_id, true);
        Destroy(gameObject);
    }
}