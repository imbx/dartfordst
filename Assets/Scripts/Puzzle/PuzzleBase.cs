using System;
using System.Collections;
using UnityEngine;
using BoxScripts;

public class PuzzleBase : InteractBase {
    public bool MoveCamera = true;

    public int EndIdentifier = 0;
    
    public float DistanceToCamera = 2f;
    public Vector3 vecModifier = Vector3.zero;
    private Vector3 CameraTarget;
    private Vector3 CameraEulerTarget;
    private BoxCollider boxCollider;

    protected override void OnStart() {
        Debug.Log("[PuzzleBase] Starting");
        OnLoad();
        if(MoveCamera) {

            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            EntityData ed = GameController.current.Player;

            Vector3 vecMultiplier = Vector3.Cross(transform.forward, vecModifier);
            CameraTarget = transform.position + vecMultiplier * DistanceToCamera;
            Vector3 invertedDirection = new Vector3(vecModifier.x * -1, vecModifier.y * -1, vecModifier.z * -1);
            CameraEulerTarget = new Vector3(
                Mathf.Rad2Deg * Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(invertedDirection.z, 2) + Mathf.Pow(invertedDirection.y, 2)), invertedDirection.x) - 90f,
                Mathf.Rad2Deg * Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(invertedDirection.z, 2) + Mathf.Pow(invertedDirection.x, 2)), invertedDirection.y) - 90f,
                Mathf.Rad2Deg * Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(invertedDirection.x, 2) + Mathf.Pow(invertedDirection.y, 2)), invertedDirection.z) - 90f);
            Debug.Log("This is destination angle " + CameraEulerTarget);
            // ed.ControlMovement(false);

            gameObject.AddComponent<CameraReposition>();
            GetComponent<CameraReposition>().SetParameters(new TransformData(CameraTarget, CameraEulerTarget));
            //DesiredPos.ApplyTo(GameController.current.mainCamera.transform);
        }
    }

    public override void OnExit() {
        base.OnExit();
        GetComponent<CameraReposition>().InvertDirection();
        boxCollider.enabled = true;
    }

    protected override void OnEnd() {
        base.OnEnd();
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