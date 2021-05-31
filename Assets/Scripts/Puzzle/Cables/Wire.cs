using UnityEngine;

public class Wire : InteractBase {

    public int WireID = -1;
    public bool isLocked = false;
    [SerializeField] private PrimaryController controller;
    public LineRenderer line;
    private bool isInteractingThis = false;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 startMousePos;

    public void SetPoint(Vector3 v)
    {
        Debug.Log("[Wire] Set Position to Knot at " + v.x + " " + v.y + " " + v.z);
        isLocked = true;
        transform.position = new Vector3(startPosition.x, v.y, v.z);
        line.SetPosition(1, new Vector3(startPosition.x, v.y, v.z));
    }

    public override void Execute(bool isLeftAction = true)
    {
        if(!isInteractingThis)
        {
            startMousePos = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            startPosition = line.GetPosition(1);
            isInteractingThis = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    } 

    private void Update()
    {

        if(isLocked)
        {
            Destroy(gameObject);
        }

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
                Debug.Log("[Wire] End moving");

                Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
                if(Physics.Raycast(
                    r, out hit, 5f,
                    LayerMask.GetMask("Focus")))
                {
                    Debug.Log(hit.collider.name);
                    //if(hit.transform.tag == "MapPin") return hit.transform;
                    if(hit.transform.GetComponent<Knot>())
                    {
                        Debug.Log("Hit knot");
                        hit.transform.GetComponent<Knot>().SetWire(this);
                    }
                }
                if(!isLocked) {
                    transform.position = startPosition;
                    line.SetPosition(1, startPosition);
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
                    startPosition.x,
                    hit.point.y,
                    hit.point.z
                );
                line.SetPosition(1, 
                new Vector3(
                        startPosition.x,
                        hit.point.y,
                        hit.point.z
                ));
            }
        }
    }
}