using System;
using System.Collections;
using UnityEngine;
using BoxScripts;

public class InteractBase : MonoBehaviour {
    public int _id = 0;
    [SerializeField] protected GameControllerObject gameControllerObject;

    private bool hasCheckedState = false;
    void OnEnable() {
        OnLoad();
    }

    private void OnLoad() {
        if(!hasCheckedState)
        {
            Debug.Log("[InteractBase] " + name);
            if(GameController.current){
                hasCheckedState = true;
                if(GameController.current.database.ProgressionExists(_id)){
                    Debug.Log("Deactivating " + name);
                    this.gameObject.SetActive(!GameController.current.database.GetProgressionState(_id));
                }
                else GameController.current.database.AddProgressionID(_id);
            }
        }
    }

    private void Update() {
        OnLoad();
    }

    protected virtual void OnStart(){

    }

    protected virtual void OnEnd() {
        if(!gameControllerObject)
            GameController.current.ChangeState(BoxScripts.GameState.ENDINTERACTING);
        else {
            gameControllerObject.ChangeState(BoxScripts.GameState.ENDINTERACTING);
            gameControllerObject.requireFocus = true;
        }
        BoxUtils.SetLayerRecursively(gameObject, 6);
    }

    public virtual void OnExit() {
        if(!gameControllerObject)
            GameController.current.ChangeState(BoxScripts.GameState.ENDINTERACTING);
        else {
            gameControllerObject.ChangeState(BoxScripts.GameState.ENDINTERACTING);
            gameControllerObject.requireFocus = true;
        }
        BoxUtils.SetLayerRecursively(gameObject, 6);
    }

    public virtual void Execute(bool isLeftAction = true){
        if(!gameControllerObject)
            GameController.current.ChangeState(GameState.INTERACTING);
        else {
            gameControllerObject.requireFocus = false;
            gameControllerObject.ChangeState(GameState.INTERACTING);
        }

        BoxUtils.SetLayerRecursively(gameObject, 8);
    }
}