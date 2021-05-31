using UnityEngine;

public class MapPin : InteractBase {
    private Map parent;
    public Transform lineStart;
    public MeshRenderer meshRenderer;
    public override void Execute(bool isLeftAction = true)
    {
        if(!isLeftAction) parent.RemovePin(gameObject);
    }

    public void SetPin(Transform tr, Material color)
    {
        this.parent = tr.GetComponent<Map>();
        Material[] mats = meshRenderer.materials;
        mats[1] = color;
        meshRenderer.materials = mats;
        transform.SetParent(tr);
    }

}