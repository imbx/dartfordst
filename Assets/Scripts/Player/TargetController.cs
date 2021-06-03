using UnityEngine;
using BoxScripts;
public class TargetController : MonoBehaviour {
    public GameControllerObject gcObject;
    public EntityData m_CVars;
    public PrimaryController m_PlayerMovement;
    private Vector3 m_ROrigin;
    private float _isPressedCd = 0f;
    [SerializeField] private float InteractRange = 2f;
    void Update() 
    {
        if(StatesToAvoid()) return;
        RaycastHit hit;
        
        m_ROrigin = gcObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 direction = gcObject.camera.transform.forward;
        if(!m_CVars.CanLook) {
            Ray mouseHit = Camera.main.ScreenPointToRay(m_PlayerMovement.Mouse);
            m_ROrigin = Camera.main.ScreenToWorldPoint(m_PlayerMovement.Mouse);
            direction = mouseHit.direction;
        }

        if(_isPressedCd > 0) _isPressedCd -= Time.deltaTime;
        LayerMask layerMask = LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable");
        if(gcObject.state == GameState.INTERACTING || gcObject.state == GameState.LOOKITEM) layerMask = LayerMask.GetMask("Focus");
        if(gcObject.targetAllLayers) layerMask = 1;
        if(Physics.Raycast(m_ROrigin, direction, out hit, m_CVars.VisionRange, layerMask)){
            

            if(Mathf.Abs((hit.transform.position - transform.position).magnitude) >
                InteractRange && !gcObject.targetAllLayers)
            {
                if(!gcObject.isInPuzzle) gcObject.playerTargetTag = "";
                if(gcObject.state == BoxScripts.GameState.TARGETING) gcObject.ChangeState(BoxScripts.GameState.PLAYING);
                GameController.current.ui.ChangeCursor(-1);
                return;
            }

            if(!gcObject.isInPuzzle) gcObject.playerTargetTag = hit.collider.tag;

            // Debug.Log("[TargetController] Hitpoint : " + hit.point);
            gcObject.playerTargetPosition = hit.point;
            bool leftButton = m_PlayerMovement.isInputPressed;
            if(
                (!m_CVars.CanLook && hit.collider.GetComponent<InteractBase>()) ||
                (hit.collider.GetComponent<InteractBase>() && Mathf.Abs((hit.transform.position - transform.position).magnitude) < InteractRange)
            )
            {
                if( gcObject.state != BoxScripts.GameState.TARGETING &&
                    gcObject.state != BoxScripts.GameState.INTERACTING &&
                    gcObject.state != BoxScripts.GameState.ENDINTERACTING &&
                    gcObject.state != BoxScripts.GameState.LOOKITEM &&
                    gcObject.state != BoxScripts.GameState.ENDLOOKITEM)
                    {
                        gcObject.playerTargetTag = hit.collider.tag;
                        switch(hit.collider.tag)
                        {
                            case "BasicInteraction":
                                GameController.current.ui.ChangeCursor(0);
                            break;
                            case "Picture":
                                GameController.current.ui.ChangeCursor(1);
                                //gcObject.ChangeState(BoxScripts.GameState.TARGETINGPICTURE);
                                // GameController.current.SetCursor();
                                break;
                            case "Locked":
                                // SHOULD CHANGE CURSOR
                                GameController.current.ui.ChangeCursor(2);
                                Debug.Log("Is locked, shouldnt do anything");
                                break;
                            case "Requirement":
                                // SHOULD CHANGE CURSOR
                                GameController.current.ui.ChangeCursor(-1);
                                Debug.Log("Is locked, shouldnt do anything");
                                break;
                            default:
                                GameController.current.ui.ChangeCursor(-1);
                                // gcObject.ChangeState(BoxScripts.GameState.TARGETING);
                                break;
                        }
                    }
                if(hit.collider.tag == "Item" || gcObject.state != BoxScripts.GameState.LOOKITEM)
                if(_isPressedCd <= 0 && (leftButton || m_PlayerMovement.isInput2Pressed)) {
                    Debug.Log("[TargetController] Executing on " + hit.collider.name);
                    _isPressedCd = 0.5f;
                    hit.collider.GetComponent<InteractBase>().Execute(leftButton);
                }
            }
        }
        else
        {
            if(!gcObject.isInPuzzle) gcObject.playerTargetTag = "";
            if(gcObject.state == BoxScripts.GameState.TARGETING) gcObject.ChangeState(BoxScripts.GameState.PLAYING);
            GameController.current.ui.ChangeCursor(-1);
        }
    }

    private bool StatesToAvoid()
    {
        return gcObject.state == GameState.MOVINGCAMERA || gcObject.state == BoxScripts.GameState.ENDINTERACTING || gcObject.state == BoxScripts.GameState.ENDLOOKITEM || gcObject.state == GameState.OPENNOTEBOOK || gcObject.state == GameState.CLOSENOTEBOOK;
    }
}