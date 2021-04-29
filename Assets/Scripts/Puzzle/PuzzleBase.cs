using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;
using BoxScripts;

public class PuzzleBase : InteractBase {
    public bool MoveCamera = true;
    
    public float DistanceToCamera = 2f;
    public Vector3 vecModifier = Vector3.zero;
    private Vector3 CameraTarget;
    private Vector3 CameraEulerTarget;
    private BoxCollider boxCollider;
    public Preset DesiredPos; // TEMPORAL, Cambiar a movimiento smooth de camara en perpendicular al puzzle en caso de MoveCamera true

    protected override void OnStart() {
        GameController.current.ToggleCursor(MoveCamera);

        if(MoveCamera) {

            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            EntityData ed = GameController.current.Player;

            Vector3 vecMultiplier = Vector3.Cross(transform.forward, vecModifier);
            CameraTarget = transform.position + vecMultiplier * DistanceToCamera;
            CameraEulerTarget = new Vector3(0, Mathf.Atan2(transform.forward.x, transform.forward.z) * 180f / Mathf.PI - 90f, 0);
            
            ed.ControlMovement(false);

            gameObject.AddComponent<CameraReposition>();
            GetComponent<CameraReposition>().SetParameters(new TransformData(CameraTarget, CameraEulerTarget));
            //DesiredPos.ApplyTo(GameController.current.mainCamera.transform);
        }
    }

    public override void OnExit() {
        GetComponent<CameraReposition>().InvertDirection();
        boxCollider.enabled = true;
    }

    protected override void OnEnd() {
        GetComponent<CameraReposition>().InvertDirection();

        GameController.current.database.EditProgression(_id);
        GameController.current.ToggleCursor(false);

        Destroy(this);
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.blue;
            Vector3 vecMultiplier = Vector3.Cross(transform.forward, vecModifier);
            Gizmos.DrawLine(
                transform.position,
                transform.position + vecMultiplier * DistanceToCamera
            );
            Gizmos.color = Color.white;
        #endif
    }
    
}