using UnityEngine;
using BoxScripts;

[CreateAssetMenu]
public class GameControllerObject : ScriptableObject {
    public GameState state = GameState.LOADGAME;
    public Camera camera;
    public bool requireFocus = true;
    public bool justChangedState = false;
    public string playerTargetTag = "";

    public void ChangeState(GameState gs)
    {
        justChangedState = true;
        Debug.Log("Requesting state change to " + gs + " and just changed? " + justChangedState);
        state = gs;
    }
}