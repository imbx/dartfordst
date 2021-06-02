using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fmod3dMusic : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string eventoSound = "event:/ambiente/Pluja";
    FMOD.Studio.EventInstance eventoInstance;

    Rigidbody cachedRigidBody;

    void Start()
    {
        eventoInstance = FMODUnity.RuntimeManager.CreateInstance(eventoSound);
        //eventoInstance.start();
    }
    void OnDestroy()
    {
        StopAllPlayerEvents();

        
        eventoInstance.release();
    }
    void StopAllPlayerEvents()
    {
        FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/player");
        playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoInstance, GetComponent<Transform>(), GetComponent<Rigidbody>());
        PlaySound();        
    }

     void PlaySound()
    {
        if (eventoInstance.isValid())
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            eventoInstance.getPlaybackState(out playbackState);
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                eventoInstance.release();
                
            }

        }
    }
}
