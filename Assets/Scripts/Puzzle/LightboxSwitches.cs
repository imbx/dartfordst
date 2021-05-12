using UnityEngine;
using UnityEngine.Events;

public class LightboxSwitches : InteractBase {
    private bool isOn = false;
    public int switchID = -1;
    public UnityEvent<int> Action;

    public override void Execute(bool isLeftAction = true) {
        if(isOn) ResetSwitch();
        else {
            isOn = true;
            transform.localEulerAngles = new Vector3(0, 0, -45f);
            Action.Invoke(switchID);
        }
    }

    public void ResetSwitch() {
        isOn = false;
        transform.localEulerAngles = new Vector3(0, 0, 45f);
        Action.Invoke(-1);
    } 

    public void OnDestroyLightbox() {
        Destroy(GetComponent<BoxCollider>());
        Destroy(this);
    }
}