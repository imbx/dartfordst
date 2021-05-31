using UnityEditor;
using UnityEngine;
using BoxScripts;

[RequireComponent(typeof(Movement))]
public class Drawer : InteractBase {
    private bool isDrawerIn = true;
    public Vector3 MovDimensions = Vector3.zero;
    public float Speed = 2f;
    private Movement movement;
    
    [FMODUnity.EventRef]
    public string abrirCajon = "event:/cajón/abrir cajón";
    FMOD.Studio.EventInstance eventOpen;

    [FMODUnity.EventRef]
    public string cerrarCajon = "event:/cajón/cerrar cajón";
    FMOD.Studio.EventInstance eventClose;

    private void Start() {
        movement = GetComponent<Movement>();
        eventOpen = FMODUnity.RuntimeManager.CreateInstance(abrirCajon);
        eventClose = FMODUnity.RuntimeManager.CreateInstance(cerrarCajon);

    }

    public override void Execute(bool isLeftAction = true)
    {
        if(movement.isAtDestination) ToggleDrawer();

        if(isDrawerIn) eventOpen.start();
        if (!isDrawerIn) eventClose.start();
    }

    void ToggleDrawer()
    {
        if(!movement.hasParameters)
        {
            movement.SetConfig(Speed);
            movement.SetParameters(
                new TransformData(
                    transform.position +
                    (isDrawerIn ? -1 : 1) * MovDimensions,
                    transform.eulerAngles),
                new TransformData(transform));
        }
        else movement.Invert();
        
        isDrawerIn = !isDrawerIn;
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                transform.position,
                transform.position + (Vector3)MovDimensions
            );
            Gizmos.color = Color.white;
        #endif
    }

}