using UnityEngine;

public class Knot : InteractBase {
    public int KnotID = -1;
    public bool hasWire = false;
    [SerializeField] private PrimaryController controller;

    public override void Execute(bool isLeftAction = true)  
    {
        base.Execute(isLeftAction);
    } 

    public void SetWire (Wire w)
    {
        if(w.WireID != KnotID) return;
        w.SetPoint(transform.position);

        hasWire = true;
        GetComponent<BoxCollider>().enabled = false;
    }
    
}