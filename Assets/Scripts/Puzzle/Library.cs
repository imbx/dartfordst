using UnityEngine;
using UnityEngine.Events;

public class Library : PuzzleBase {
    public string Combination = "2341";
    [SerializeField]private string CurrentCombination = "";

    public UnityEvent OnResetLightbox;

    void Start(){
        // this.OnStart();
    }

    void Update() {
        if(Combination.Equals(CurrentCombination))
            this.OnEnd();
    }

    void ResetLightbox() {
        Debug.Log("Reset Comb");
        CurrentCombination = "";
        OnResetLightbox.Invoke();
    }

    public void AddToCombination (string newLever)
    {
        if(Combination[CurrentCombination.Length] == newLever[0])
            CurrentCombination += newLever;
        else ResetLightbox();
    }
    
}