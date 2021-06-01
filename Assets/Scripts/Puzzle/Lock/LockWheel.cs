using UnityEngine;

public class LockWheel : InteractBase {
    private bool isInteractingThis = false;
    private bool isHold = false;
    private float CurrentRotation;
    private float RotationLerp = 0;
    [Header("Rounded Lock Parameters")]
    [SerializeField] private PrimaryController controller;
    public Vector2 Interval = new Vector2(0, 9f);

    public int Number = 0;

    [FMODUnity.EventRef]
    public string eventoSound = "event:/weels2d";

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);
        isInteractingThis = true;
        GameController.current.music.playMusic(eventoSound);

    }
    private void Update() {
        if(!isInteractingThis) return;
        else if(!(controller.isInput2Pressed || controller.isInputPressed))
        {
            // transform.localEulerAngles = new Vector3(CurrentRotation * 360f / 9, 0, 0);
            isInteractingThis = false;
            isHold = false;
            return;
        }
        isHold = controller.isInputHold || controller.isInput2Hold;


        int mouseDir = -BoxScripts.BoxUtils.ConvertTo01((int)controller.CameraAxis.y);
        CurrentRotation = (int) Mathf.Lerp(Interval.y, Interval.x, RotationLerp);
        RotationLerp += mouseDir * 0.35f * Time.deltaTime;

        if(RotationLerp > 1f ) RotationLerp = 0f;
        if(RotationLerp < 0f) RotationLerp = 1f;

        Number = (int) CurrentRotation;

        transform.localEulerAngles = new Vector3((int)Mathf.Lerp(0, 360f, RotationLerp), 0, 0);

        // Debug.Log("[LookWheel] Current rot : " + CurrentRotation);
    }

}