using System;
using System.Collections;
using UnityEngine;
using BoxScripts;
public class PuzzleBase : InteractBase {
    [Header("Puzzle Base Parameters")]
    public bool MoveCamera = true;

    public int EndIdentifier = 0;
    
    public float DistanceToCamera = 2f;
    public Vector3 vecModifier = Vector3.zero;
    public Vector3 targetAngle = Vector3.zero;
    private Vector3 CameraTarget;
    private Vector3 CameraEulerTarget;
    private BoxCollider boxCollider;
    protected bool isInteractingThis = false;

    public PrimaryController controller;

    protected override void OnStart() {
        Debug.Log("[PuzzleBase] Starting");
        if(MoveCamera) {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            GameController.current.MoveCamera(transform.position, (vecModifier * DistanceToCamera), targetAngle);
        }
    }

    public override void Execute(bool isLeftAction = true)
    {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        base.Execute(isLeftAction);
        this.OnStart();
        isInteractingThis = true;
        gameControllerObject.isInPuzzle = true;
    }


    public override void OnExit() {
        base.OnExit();
        //GetComponent<CameraReposition>().InvertDirection();
        isInteractingThis = false;
        gameControllerObject.isInPuzzle = false;
        GameController.current.RevertCamera();
        boxCollider.enabled = true;
    }

    protected override void OnEnd(bool destroyGameObject = false) {
        base.OnEnd();
        // GetComponent<CameraReposition>().InvertDirection();
        isInteractingThis = false;
        gameControllerObject.isInPuzzle = false;
        GameController.current.RevertCamera();
        Debug.Log("[PuzzleBase] Activating progress id : " + _id);
        GameController.current.database.EditProgression(_id);
        if(destroyGameObject) Destroy(gameObject);
        DestroyInteraction();
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                transform.position,
                transform.position + vecModifier * DistanceToCamera
            );
            Gizmos.color = Color.white;
        #endif
    }
    
}