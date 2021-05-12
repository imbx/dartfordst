using UnityEngine;
using BoxScripts;

public class GrabbableItem : InteractBase
{
    private TransformData from;
    private TransformData target;
    private Transform playerCamera;
    [SerializeField] private bool hasParameters = false;
    [SerializeField] private float timer = 0;
    public float speed = 2f;
    private bool isReturning = false;
    private bool isAtDestination = false;
    private bool isLeftAction = true;
    [SerializeField] private PrimaryController controller;

    public override void Execute(bool isLeftAction = true) {
        base.Execute();
        playerCamera = GameController.current.Hand;
        target = new TransformData(playerCamera.transform);
        SetParameters(transform);

        Debug.Log("Is here");

        if(isAtDestination) {
            if(isLeftAction)
            {
                base.OnExit();
                Debug.Log("Should destroy now");
                GameController.current.database.AddProgressionID(_id, true);
                Destroy(gameObject);
                return;
            }
            this.isLeftAction = isLeftAction;
        }
    }

    private void Update()
    {
        if(hasParameters)
        { 
            if(timer < 1f)
                timer += Time.deltaTime * speed;
            else {
                isAtDestination = true;
                hasParameters = false;
                if(isReturning) {
                    GameController.current.Player.ControlMovement(true);
                    Destroy(this);
                }
            }

            transform.eulerAngles = from.LerpAngle(target, timer);
            transform.position = from.LerpDistance(target, timer);
        }

        if(isAtDestination && !isLeftAction)
        {
            if(controller.isInput2Hold)
            {
                transform.localEulerAngles += (Vector3)controller.CameraAxis;
            }
        }
    }

    public void SetParameters(Transform targetDestination)
    {
        from = new TransformData(targetDestination);
        hasParameters = true;
        gameObject.layer = 8;
    }

    public void SetParameters(TransformData targetDestination)
    {
        from = targetDestination;
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