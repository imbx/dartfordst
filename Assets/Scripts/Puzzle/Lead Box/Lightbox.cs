using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lightbox : PuzzleBase {
    [Header("Lead Box Parameters")]
    public List<int> FinalCombination;
    public UnityEvent<bool> OnResetLightbox;
    public UnityEvent OnDestroyLightbox;
    private List<int> CombinationList;

    //[FMODUnity.EventRef]
    //public string eventoSound = "event:/interruptor2d";
    

    private void Start() {
        CombinationList = new List<int>();
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        base.OnEnd();
        GetComponent<LightController>().LightController_AnimLights();
        OnDestroyLightbox.Invoke();
    }

    void ResetLightbox() {
        CombinationList = new List<int>();
        OnResetLightbox.Invoke(true);
    }
    private void Update() {
        if(hasRequirement)
        {
            if(GameController.current.database.GetProgressionState(reqID))
                transform.tag = "BasicInteraction";
        }
        if(!isInteractingThis) return;

        if(controller.isEscapePressed)
        {
            Debug.Log("[Lightbox] Escape pressed");
            this.OnExit();
        }

    }

    public void AddToCombination (int newLever)
    {
        if(CombinationList.Contains(newLever)) CombinationList.Remove(newLever);
        else CombinationList.Add(newLever);
    }

    private bool CompareLists()
    {
        if(FinalCombination.Count != CombinationList.Count) return false;
        foreach(int i in FinalCombination)
            if(!CombinationList.Contains(i)) return false;
        return true;
    }

    public void CheckCombination(int newLever = 0)
    {
        //GameController.current.music.playMusic(eventoSound);
        if (CompareLists())
            this.OnEnd();
        else ResetLightbox();
    }
}