using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;

public class PuzzleBase : InteractBase {
    public bool MoveCamera = true;
    public Preset DesiredPos; // TEMPORAL, Cambiar a movimiento smooth de camara en perpendicular al puzzle en caso de MoveCamera true

    protected override void OnStart() {

        GameController.current.ToggleCursor(MoveCamera);

        if(MoveCamera) {
            DesiredPos.ApplyTo(GameController.current.mainCamera.transform);
        }
    }

    protected override void OnEnd() {
        GameController.current.database.EditProgression(_id);
        GameController.current.ToggleCursor(false);
    }
    
}