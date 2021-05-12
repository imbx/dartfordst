using UnityEngine;
using BoxScripts;

[RequireComponent(typeof (Movement))]
public class Item : InteractBase {
    public bool NoEffects = false;
    public bool CanPickup = true;
    public bool HasItemInside = false;
    private bool isLeftAction = true;
    [SerializeField] private PrimaryController controller;
    private Movement movement;
    private TransformData startTransform;
    [SerializeField] private GameObject Son = null;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public override void Execute(bool isLeftAction = true) {
        Debug.Log("Execute");
        if(isLeftAction && gameControllerObject.state != GameState.LOOKITEM){
            if(NoEffects) {
                OnEnd();
                return;
            }
            if(startTransform == null) 
            {
                startTransform = new TransformData(transform);
                movement.SetConfig(2f, true);
                movement.SetParameters(new TransformData(GameController.current.Hand), startTransform);
            } else movement.Invert();
            if(HasItemInside) GetComponent<BoxCollider>().enabled = false;
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
        isLeftAction = false;
        GameController.current.database.AddProgressionID(_id, true);
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        if(NoEffects || CanPickup) Destroy(gameObject);
    }

    public override void OnExit()
    {
        // base.OnExit();
        movement.Invert();
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        if(HasItemInside) {
            if(Son) GetComponent<BoxCollider>().enabled = true;
            else this.enabled = false;
        }
    }

    private void Update()
    {
        if(movement.isAtDestination)
        {
            if(HasItemInside && Son == null) OnExit();
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