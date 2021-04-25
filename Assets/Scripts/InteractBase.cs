using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;

public class InteractBase : MonoBehaviour {
    public int _id;
    void OnEnable() {
        if(GameController.current.database.ProgressionExists(_id)){
            this.gameObject.SetActive(!GameController.current.database.GetProgressionState(_id));
        }
        else GameController.current.database.AddProgressionID(_id);
    }

    protected virtual void OnStart(){

    }

    protected virtual void OnEnd() {
    }
}