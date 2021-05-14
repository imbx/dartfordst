using BoxScripts;
using UnityEngine;

[RequireComponent(typeof (Movement))]
public class CameraMovement : MonoBehaviour {

    public GameControllerObject gameControllerObject;
    public Transform pointerToLook;
    private Transform parent;
    private Movement movement;
    private TransformData transformData;
    private TransformData pointerData;
    private Vector3 rotation;

    private bool isMoving = false;

    private bool sentCameraAway = false;

    private void Awake() {
        movement = GetComponent<Movement>();
    }

    private void OnEnable() {
        CopyTransform();
    }

    public void PrepareMovement(Vector3 destination, Vector3 modifier, Vector3 rotation)
    {
        if(gameControllerObject.state != GameState.MOVINGCAMERA) gameControllerObject.ChangeState(GameState.MOVINGCAMERA);
        sentCameraAway = !sentCameraAway;
        isMoving = true;
        CopyTransform();
        pointerToLook.GetComponent<Movement>().SetConfig(2f);
        pointerToLook.GetComponent<Movement>().SetParameters(new TransformData(destination, rotation));
        movement.SetConfig(2f);
        movement.SetParameters(new TransformData(destination + modifier, rotation));
    }
    
    public void RevertCamera()
    {
        gameControllerObject.ChangeState(GameState.MOVINGCAMERA);
        sentCameraAway = !sentCameraAway;
        isMoving = true;
        pointerToLook.GetComponent<Movement>().Invert();
        movement.Invert();
    }

    private void CopyTransform()
    {
        transformData = new TransformData(transform);
        pointerData = new TransformData(pointerToLook);
    }

    private void Update()
    {
        if(!isMoving && gameControllerObject.state == GameState.MOVINGCAMERA) {
            if(!sentCameraAway){
                gameControllerObject.ChangeState(GameState.PLAYING);
                transform.position = transformData.position;
                transform.eulerAngles = transformData.eulerAngles;
                pointerToLook.position = pointerData.position;
            }
            else gameControllerObject.ChangeState(GameState.INTERACTING);
            
        }
        if(isMoving) {
            isMoving = movement.isAtDestination;
        }
        transform.LookAt(pointerToLook);
    }
    
}