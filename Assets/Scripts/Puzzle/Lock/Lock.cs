using System.Collections.Generic;
using UnityEngine;

public class Lock : PuzzleBase {

    public List<LockWheel> keys;

    public string FinalCombination = "123";

    [FMODUnity.EventRef]
    public string candadoSound = "event:/candado";
    FMOD.Studio.EventInstance lockEvent;

    Rigidbody cachedRigidBody;


    private void Start()
    {
        cachedRigidBody = GetComponent<Rigidbody>();
        lockEvent = FMODUnity.RuntimeManager.CreateInstance(candadoSound);
        lockEvent.start();
    }


    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        lockEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));

        if (GetCurrentCombination().Equals(FinalCombination))
        { 
            this.OnEnd(true);
            
            if (lockEvent.isValid())
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                lockEvent.getPlaybackState(out playbackState);
                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    lockEvent.release();
                    lockEvent.clearHandle();
                    SendMessage("lockEventFinished");
                }
            }

        }

    }

    private string GetCurrentCombination()
    {
        string combination = "";

        foreach(LockWheel lcc in keys)
        {
            combination += lcc.Number.ToString();
            
        }
        Debug.Log("[Lock] Current combination is " + combination);
        return combination;
    }
    
}