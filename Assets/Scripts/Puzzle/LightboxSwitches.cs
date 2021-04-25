using UnityEngine;
using UnityEngine.Events;

public class LightboxSwitches : MonoBehaviour {
    private bool isOn = false;
    public UnityEvent Action;

    public Material Red;
    public Material Green; // ONLY FOR TEST PURPOSES

    public void ToggleSwitch() {
        if(isOn) ResetSwitch();
        else {
            isOn = true;
            Action.Invoke();
            GetComponent<MeshRenderer>().material = Green;
        }
    }

    public void ResetSwitch() {
        isOn = false;
        GetComponent<MeshRenderer>().material = Red;
    } 
}