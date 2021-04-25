using UnityEngine;
using UnityEngine.Events;

public class Lightbox : PuzzleBase {
    public string Combination = "2341";
    private string CurrentCombination = "";

    public UnityEvent OnResetLightbox;

    void Start(){
        this.OnStart();
    }

    void Update() {
        if(Combination.Equals(CurrentCombination))
            this.OnEnd();
    }

    void ResetLightbox() {
        CurrentCombination = "";
        OnResetLightbox.Invoke();
    }

    public void AddToCombination (string newLever)
    {
        if(Combination[CurrentCombination.Length].Equals(newLever))
            CurrentCombination += newLever;
        else ResetLightbox();
    }
}