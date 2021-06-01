using UnityEngine;
using UnityEngine.Events;

public class Note : InteractBase {

    [SerializeField] private PrimaryController controller;

    public int Identifier {
        get {
            return _id;
        }
    }
    [SerializeField] private bool hasPressedLeft = false;
    [SerializeField] private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 startMousePos = Vector3.zero;

    private BoxCollider boxCollider;

    public UnityEvent<Note> OnAction;

    private float distance = 0f;

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
        if(!controller.isInputHold){
            hasPressedLeft = isLeftAction;
            this.OnStart();
            GameController.current.music.playMusic(itemSound);
        }
    }

    void Update()
    {
        if(hasPressedLeft && !isMoving)
        {
            distance = Vector3.Distance(transform.parent.position, Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f)));
            startMousePos = Camera.main.ScreenToWorldPoint(new Vector3(controller.Mouse.x, controller.Mouse.y, distance));
            Debug.Log("[Note] START MOUSE POS IS " + startMousePos);
            isMoving = controller.isInputHold;
        }
        else if(hasPressedLeft && isMoving)
        {
            if(!controller.isInputPressed)
            {
                isMoving = false;
                hasPressedLeft = false;
                // gameControllerObject.ChangeState(BoxScripts.GameState.PLAYING);
                Debug.Log("[Note] Not moving1 "  + gameObject.name);

                RaycastHit hit;
                Ray mouseHit = Camera.main.ScreenPointToRay(controller.Mouse);
                Vector3 m_ROrigin = Camera.main.ScreenToWorldPoint(controller.Mouse);
                Vector3 direction = mouseHit.direction;
                Debug.Log("[Note] Prepare to hit raycast with direction " + direction );
                if(Physics.Raycast(m_ROrigin, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Focus"))){
                    Debug.Log("[Note] Hitting note raycast");
                    if(hit.collider.GetComponent<NotePlacement>())
                    {

                        Debug.Log("[Note] Found placement");
                        NotePlacement np = hit.collider.GetComponent<NotePlacement>();
                        if(np.Identifier == _id)
                        {
                            transform.position = np.transform.position;
                            OnAction.Invoke(this);
                        }
                    }
                    Debug.Log(hit.collider.name);
                }
                else transform.position = startPosition;
                boxCollider.enabled = true;
            }
        }

        if(isMoving)
        {
            isMoving = controller.isInputHold;
            Debug.Log("[Note] Moving " + gameObject.name);
            Debug.Log(isMoving + " " + gameObject.name);

            if(boxCollider.enabled) boxCollider.enabled = false;

            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(controller.Mouse.x, controller.Mouse.y, distance));
            Debug.Log("[Note] Difference should be " + (float)(vec.x - startMousePos.x) + " " + (float)(vec.y - startMousePos.y) + " from x " + vec.x + " y " + vec.y + " and startpos " + startMousePos);
            transform.position =
                new Vector3(
                    startPosition.x + (float)(vec.x - startMousePos.x),
                    startPosition.y ,
                    startPosition.z + (float)(vec.z - startMousePos.z)
                );
            
            if(!isMoving)
            {
                Debug.Log("[Note] Not moving "  + gameObject.name);
                
            }
        }
    }

}