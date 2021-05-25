using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lightbox : PuzzleBase {
    [Header("Lead Box Parameters")]
    public List<int> FinalCombination;
    public MeshRenderer BulbRenderer;
    public Material BulbEndMaterial;

    //[SerializeField] private string CurrentCombination = "";
    public UnityEvent<bool> OnResetLightbox;
    public UnityEvent OnDestroyLightbox;
    private List<int> CombinationList;

    void Update() {
        // if(CurrentCombination.Length == 0f) ResetLightbox();
    }

    private void Start() {
        CombinationList = new List<int>();
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        base.OnEnd();
        OnDestroyLightbox.Invoke();
        BulbRenderer.material = BulbEndMaterial;
    }

    void ResetLightbox() {
        // CurrentCombination = "";
        CombinationList = new List<int>();
        OnResetLightbox.Invoke(true);
    }

    /*void ResetCombination() {
        CurrentCombination = "";
    }*/

    public void AddToCombination (int newLever)
    {
        if(CombinationList.Contains(newLever)) CombinationList.Remove(newLever);
        else CombinationList.Add(newLever);
        
        /*if(Combination[CurrentCombination.Length].Equals(newLever.ToString()[0]))
        {
            CurrentCombination += newLever.ToString();
            
        }
        else ResetCombination();*/
    }

    private bool CompareLists()
    {
        foreach(int i in FinalCombination)
            if(!CombinationList.Contains(i)) return false;
        return true;
    }

    public void CheckCombination(int newLever = 0)
    {
        if(CompareLists())
            this.OnEnd();
        else ResetLightbox();
    }
}