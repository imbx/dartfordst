using UnityEngine;
using UnityEngine.Events;

public class Lightbox : PuzzleBase {
    public string Combination = "2341";
    public MeshRenderer BulbRenderer;
    public Material BulbEndMaterial;
    [SerializeField] private string CurrentCombination = "";

    public UnityEvent OnResetLightbox;
    public UnityEvent OnDestroyLightbox;

    void Update() {
        // if(CurrentCombination.Length == 0f) ResetLightbox();
        if(Combination.Equals(CurrentCombination))
            this.OnEnd();
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        base.OnEnd();
        OnDestroyLightbox.Invoke();
        BulbRenderer.material = BulbEndMaterial;
    }

    void ResetLightbox() {
        CurrentCombination = "";
        OnResetLightbox.Invoke();
    }

    void ResetCombination() {
        CurrentCombination = "";
    }

    public void AddToCombination (int newLever)
    {
        if(Combination[CurrentCombination.Length].Equals(newLever.ToString()[0]))
            CurrentCombination += newLever.ToString();
        else ResetCombination();
    }
}