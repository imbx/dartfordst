using UnityEngine;
using UnityEngine.Events;

public class Note : InteractBase {

    [SerializeField] private PrimaryController controller;

    public int Identifier {
        get {
            return _id;
        }
    }
    [SerializeField] private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 startMousePos = Vector3.zero;
    public UnityEvent<Note> OnAction;

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";




    private bool isInteractingThis = false;

    public override void Execute(bool isLeftAction = true)
    {
        if(controller.isInputHold || controller.isInput2Hold) return;
        if(!isInteractingThis)
        {
            startMousePos = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            startPosition = transform.position;
            isInteractingThis = true;
            GetComponent<BoxCollider>().enabled = false;
            GameController.current.music.playMusic(itemSound);
        }
    }

    void Update()
    {
        if(isInteractingThis && !isMoving)
        {
            isMoving = controller.isInputHold || controller.isInput2Hold;
        }
        else if(isInteractingThis && isMoving)
        {
            if(!controller.isInputPressed)
            {
                isMoving = false;
                isInteractingThis = false;
                RaycastHit hit;
                Vector3 m_ROrigin = gameControllerObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                Vector3 direction = gameControllerObject.camera.transform.forward;
                Debug.Log("[Note] End moving");

                Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
                if(Physics.Raycast(
                    r, out hit, 5f,
                    LayerMask.GetMask("Focus")))
                {
                    if(hit.collider.GetComponent<NotePlacement>())
                    {

                        Debug.Log("[Note] Found placement");
                        NotePlacement np = hit.collider.GetComponent<NotePlacement>();
                        if(np.Identifier == _id)
                        {
                            transform.position = np.transform.position;
                            OnAction.Invoke(this);
                        }
                        else
                        {
                            transform.position = startPosition;
                            GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                else 
                {
                    transform.position = startPosition;
                    GetComponent<BoxCollider>().enabled = true;
                }
            }
        }

        if(isMoving)
        {
            isMoving = controller.isInputHold;
            Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
            if(Physics.Raycast(
                r, out var hit, 5f))
            {
                transform.position = new Vector3(
                    hit.point.x,
                    startPosition.y,
                    hit.point.z
                );
            }
        }




    }

}