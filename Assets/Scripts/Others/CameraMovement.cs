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

    [SerializeField] private bool isMoving = false;

    [SerializeField] private bool sentCameraAway = false;

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
        Vector3 pointerDestination = destination + ((destination - pointerToLook.transform.position).normalized * 0.01f);
        pointerToLook.GetComponent<Movement>().SetParameters(new TransformData(pointerDestination, rotation));
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
                Debug.Log("[CameraMovement] Set base pos and rot");
                transform.localPosition = new Vector3(0, 0.6f, 0);
                transform.localEulerAngles = Vector3.zero;
                pointerToLook.localPosition = new Vector3(0, 0.6f, 0.25f);
                gameControllerObject.ChangeState(GameState.PLAYING);
            }
            else gameControllerObject.ChangeState(GameState.INTERACTING);
            
        }

        if(isMoving) {
            isMoving = !movement.isAtDestination;
        }
        
        transform.LookAt(pointerToLook);
    }
    
}