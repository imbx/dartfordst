using UnityEngine;
using BoxScripts;

[CreateAssetMenu]
public class GameControllerObject : ScriptableObject {
    public GameState state = GameState.LOADGAME;
    public Camera camera;
    public bool requireFocus = true;
    public bool justChangedState = false;
    public string playerTargetTag = "";
    public Vector3 playerTargetPosition = Vector3.zero;

    public bool targetAllLayers = false;
    public bool isInPuzzle = false;

    public void ChangeState(GameState gs)
    {
        justChangedState = true;
        Debug.Log("Requesting state change to " + gs + " and just changed? " + justChangedState);
        state = gs;
    }
}