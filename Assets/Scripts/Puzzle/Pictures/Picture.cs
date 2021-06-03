using UnityEngine;
using UnityEngine.Events;
public class Picture : InteractBase {

    [SerializeField] private PrimaryController controller;

    public int Identifier {
        get {
            return _id;
        }
    }
    private bool hasPressedLeft = false;
    private bool isMoving = false;
    private Vector3 startPosition;
    [SerializeField] private Vector3 finalRotation;
    private Vector3 startMousePos = Vector3.zero;
    [SerializeField] private float forcedRotation = 30f;

    private BoxCollider boxCollider;

    public UnityEvent<Picture> OnAction;

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";

    protected override void OnStart()
    {
        boxCollider = GetComponent<BoxCollider>();
        startPosition = transform.position;
        startMousePos = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
    }

    public override void Execute(bool isLeftAction = true)
    {
        if(!(controller.isInputHold || controller.isInput2Hold)){
            hasPressedLeft = isLeftAction;
            this.OnStart();
            GameController.current.music.playMusic(itemSound);

            if (!isLeftAction)
            {
                transform.Rotate(Vector3.forward * forcedRotation);
            }
        }
    }

    void Update()
    {
        if(hasPressedLeft && !isMoving)
        {
            isMoving = controller.isInputHold;
        }
        else if(hasPressedLeft && isMoving)
        {
            if(!controller.isInputPressed)
            {
                isMoving = false;
                hasPressedLeft = false;
                gameControllerObject.ChangeState(BoxScripts.GameState.PLAYING);
                gameControllerObject.targetAllLayers = false;
                gameControllerObject.isInPuzzle = false;

                RaycastHit hit;
                Vector3 m_ROrigin = gameControllerObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                Vector3 direction = gameControllerObject.camera.transform.forward;

                if(Physics.Raycast(m_ROrigin, direction, out hit, 50f, LayerMask.GetMask("Interactuable"))){
                    Debug.Log(hit.collider.name);
                    ChangePosition(hit.collider.GetComponent<Picture>().ChangePosition(startPosition));
                }
                else transform.localPosition = startPosition;
                boxCollider.enabled = true;
            }
        }

        if(isMoving)
        {
            if(gameControllerObject.state != BoxScripts.GameState.MOVINGPICTURE) {
                gameControllerObject.ChangeState(BoxScripts.GameState.MOVINGPICTURE);
                gameControllerObject.targetAllLayers = true;
                gameControllerObject.isInPuzzle = true;
            }
            
            isMoving = controller.isInputHold;
            if(boxCollider.enabled) boxCollider.enabled = false;
            transform.position = new Vector3(startPosition.x + 0.25f, startPosition.y, gameControllerObject.playerTargetPosition.z);
        }
    }

    public Vector3 ChangePosition (Vector3 newPosition)
    {
        startPosition = transform.position;
        Vector3 oldPos = startPosition;
        transform.position = newPosition;
        startPosition = transform.position;
        OnAction.Invoke(this);
        return oldPos;
    }

    public bool CheckRotation()
    {
        return Mathf.Abs(transform.localEulerAngles.z) == Mathf.Abs(finalRotation.z);
    }
    
}