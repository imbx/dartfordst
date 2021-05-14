using System;
using System.Collections;
using UnityEngine;
using BoxScripts;

public class InteractBase : MonoBehaviour {
    public int _id = 0;

    public bool hasRequirement = false;
    public bool hideIfReq = false;
    public int reqID = -1;
    [SerializeField] protected GameControllerObject gameControllerObject;

    private bool hasCheckedState = false;

    private void Awake() {
        GameController.current.SubscribeInteraction(this);
    }
    void OnEnable()
    {
    }

    public void OnLoad() {
        if(!hasCheckedState)
        {
            Debug.Log("[InteractBase] " + name);
            if(GameController.current){
                if(hasRequirement && !GameController.current.database.GetProgressionState(reqID))
                {
                    tag = "Requirement";
                    if(hideIfReq) gameObject.SetActive(false);
                }

                hasCheckedState = true;
                if(_id != 0)
                    if(GameController.current.database.ProgressionExists(_id)){
                        Debug.Log("Deactivating " + name);
                        // this.gameObject.SetActive(!GameController.current.database.GetProgressionState(_id));
                    }
                    else GameController.current.database.AddProgressionID(_id);
            }
        }
    }

    private void Update() {
    }

    protected virtual void OnStart(){

    }

    protected virtual void OnEnd(bool destroyGameObject = false) {
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
        
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        
        if(!gameControllerObject)
            GameController.current.ChangeState(GameState.INTERACTING);
        else {
            gameControllerObject.requireFocus = false;
            gameControllerObject.ChangeState(GameState.INTERACTING);
        }

        BoxUtils.SetLayerRecursively(gameObject, 8);
    }
}