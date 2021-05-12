using UnityEngine;
using BoxScripts;

[RequireComponent(typeof (Movement))]
public class Item : InteractBase {

    public bool CanPickup = true;
    private bool isLeftAction = true;
    [SerializeField] private PrimaryController controller;
    private Movement movement;
    private TransformData startTransform;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public override void Execute(bool isLeftAction = true) {
        if(isLeftAction && gameControllerObject.state != GameState.LOOKITEM){
            if(startTransform == null) 
            {
                startTransform = new TransformData(transform);
                movement.SetConfig(2f, true);
                movement.SetParameters(new TransformData(GameController.current.Hand), startTransform);
            } else movement.Invert();
            
            gameControllerObject.ChangeState(GameState.LOOKITEM);
        }

        if(movement.isAtDestination) {
            if(isLeftAction && CanPickup)
            {
                OnEnd();
                return;
            }
            this.isLeftAction = isLeftAction;
        }
    }

    protected override void OnEnd()
    {
        Debug.Log("Should destroy now");
        GameController.current.database.AddProgressionID(_id, true);
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        Destroy(gameObject);
    }

    public override void OnExit()
    {
        base.OnExit();
        movement.Invert();
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
    }

    private void Update()
    {
        if(movement.isAtDestination)
        {
            if(!isLeftAction && controller.isInput2Hold)
            {
                transform.localEulerAngles += new Vector3(-controller.CameraAxis.y, controller.CameraAxis.x, 0);
            }
            else if(isLeftAction)
            {
                isLeftAction = false;

                
            }
        }

        if(gameControllerObject.state == GameState.LOOKITEM && controller.isEscapePressed)
            OnExit();
    }
    
}