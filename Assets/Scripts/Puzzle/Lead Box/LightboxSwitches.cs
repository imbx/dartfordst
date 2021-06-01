using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LightboxSwitches : InteractBase {
    private bool isOn = false;
    public int switchID = -1;

    public bool isChecker = false;
    public UnityEvent<int> Action;

    [FMODUnity.EventRef]
    public string interruptorSound = "event:/interruptor2d";
    [FMODUnity.EventRef]
    public string palancaSound = "event:/palanca2d";


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
            GameController.current.music.playMusic(interruptorSound);//sonido interruptor pequeño
            Action.Invoke(switchID);
        }
    }

    public void ResetSwitch(bool isReset = false) {
        isOn = false;

        if (isChecker) 
        { 
            transform.localEulerAngles = new Vector3(0, 0, 0);
            GameController.current.music.playMusic(palancaSound);//grande
        }
        
        else
        { 
            transform.localEulerAngles = new Vector3(0, 45f, 0);
            GameController.current.music.playMusic(interruptorSound);//sonido interruptor pequeño
        }
        if (!isReset) Action.Invoke(switchID);
    } 

    public void OnDestroyLightbox() {
        Destroy(GetComponent<BoxCollider>());
        Destroy(this);
    }

    private IEnumerator Animate()//palanca grande
    {
        float timer = 0;
        while(timer < 1f)
        {
            if(timer < 1f)
                timer += Time.deltaTime * 5f;

            transform.localEulerAngles = Vector3.up * Mathf.LerpAngle(0, 180f, timer);
                        
            yield return null;
        }
        GameController.current.music.playMusic(palancaSound);//sonido de palanca grande
        yield return new WaitForSeconds(0.25f);
        Action.Invoke(switchID);
        yield return null;
    }
}
