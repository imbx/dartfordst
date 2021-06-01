using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class MusicControler : MonoBehaviour
{
    private EventInstance soundEvent;

    public void playMusic(string fmodstring)
    {
        //Studio
        soundEvent = RuntimeManager.CreateInstance(fmodstring);
        soundEvent.start();
         
    }

    public void StopMusic(string fmodstring)
    {
        
        soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
