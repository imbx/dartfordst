using UnityEditor;
using UnityEngine;

public class Drawer : InteractBase {

    [SerializeField]
    private Vector3 Position = Vector3.zero;
    [SerializeField]
    private Vector2 Size = Vector2.one;

    [SerializeField]
    private float Margin = 0.05f;

    [SerializeField]
    private bool hasToMove = false;
    private bool isDrawerIn = true;
    public bool HasToMove { get { return hasToMove; } } // From level loader, check last block if its placed with this
    public Vector3 maxMovementSize = Vector3.zero;
    public float movementBlockSpeed = 2f;
    private Vector3 transformedMovement = Vector3.zero;

    void Awake() { 
        Position = transform.position;
    }

    public void Move(){ 
        hasToMove = true; 
    }

    public override void Execute()
    {
        ToggleDrawer();
    }

    void Update()
    {
        if(hasToMove) {
            if(isDrawerIn) transformedMovement += maxMovementSize * Time.deltaTime * movementBlockSpeed;
            else transformedMovement -= maxMovementSize * Time.deltaTime * movementBlockSpeed;
            
            if(Mathf.Abs(transformedMovement.z) - Margin < 0 && !isDrawerIn) {
                isDrawerIn = true;
                hasToMove = false;
            }
            
            if(Mathf.Abs(transformedMovement.z) + Margin > Mathf.Abs(maxMovementSize.z) && isDrawerIn) {
                isDrawerIn = false;
                hasToMove = false;
            }

            transform.position = Position + (Vector3)transformedMovement;
        }

    }

    void ToggleDrawer()
    {
        hasToMove = true;  
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                transform.position,
                transform.position + (Vector3)maxMovementSize
            );
            Gizmos.color = Color.white;
        #endif
    }

}