
using UnityEngine;
using BoxScripts;

public class CameraReposition : MonoBehaviour {
    private TransformData from;
    private TransformData target;
    private Camera playerCamera;

    private bool hasParameters = false;
    private float timer = 0;
    public float speed = 2f;

    private bool isReturning = false;

    private void OnEnable() {
        playerCamera = Camera.main;
        from = new TransformData(playerCamera.transform);
    }

    private void Update()
    {
        if(hasParameters)
        { 
            if(timer < 1f)
                timer += Time.deltaTime * speed;
            else {
                hasParameters = false;
                if(isReturning) {
                    GameController.current.Player.ControlMovement(true);
                    Destroy(this);
                    return;
                }
            }

            playerCamera.transform.eulerAngles = from.LerpAngle(target, timer);
            playerCamera.transform.position = from.LerpDistance(target, timer);
        }
    }

    public void SetParameters(Transform targetDestination)
    {
        target = new TransformData(targetDestination);
        hasParameters = true;
    }

    public void SetParameters(TransformData targetDestination)
    {
        target = targetDestination;
        hasParameters = true;
    }

    public void InvertDirection ()
    {
        timer = 0;
        TransformData tempTransform = from;
        from = target;
        target = tempTransform;
        hasParameters = true;
        isReturning = true;
    }
}