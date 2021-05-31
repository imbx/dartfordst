using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LightboxSwitches : InteractBase {
    private bool isOn = false;
    public int switchID = -1;

    public bool isChecker = false;
    public UnityEvent<int> Action;

    [FMODUnity.EventRef]
    public string interruptorSound = "event:/interruptor";
    FMOD.Studio.EventInstance interruptorEvent;

    private void Start()
    {
        interruptorEvent = FMODUnity.RuntimeManager.CreateInstance(interruptorSound);
    }
    public override void Execute(bool isLeftAction = true) {
        if(isOn) ResetSwitch();
        else {
            isOn = true;
            if(isChecker) {
                transform.localEulerAngles = new Vector3(0, 180f, 0);
                StartCoroutine(Animate());
                return;
            }
            transform.localEulerAngles = new Vector3(0, 90 + 45f, 0);
            Action.Invoke(switchID);
        }
    }

    public void ResetSwitch(bool isReset = false) {
        isOn = false;
        
        if(isChecker) transform.localEulerAngles = new Vector3(0, 0, 0);
        else transform.localEulerAngles = new Vector3(0, 45f, 0);
        if(!isReset) Action.Invoke(switchID);
    } 

    public void OnDestroyLightbox() {
        Destroy(GetComponent<BoxCollider>());
        Destroy(this);
    }

    private IEnumerator Animate()
    {
        float timer = 0;
        while(timer < 1f)
        {
            if(timer < 1f)
                timer += Time.deltaTime * 5f;

            interruptorEvent.start();//sound

            transform.localEulerAngles = Vector3.up * Mathf.LerpAngle(0, 180f, timer);
                        
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        Action.Invoke(switchID);
        yield return null;
    }
}
