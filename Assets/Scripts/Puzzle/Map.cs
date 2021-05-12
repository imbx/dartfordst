using UnityEngine;
using System.Collections.Generic;
using BoxScripts;

public class Map : InteractBase {
    private List<Vector3> Pointers;
    private Vector3 Marker = Vector3.zero;
    private MapInteractions MapState = MapInteractions.None;
    [SerializeField] private PrimaryController controller;

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);

        switch(MapState)
        {
            case MapInteractions.Add:
            
                break;
            case MapInteractions.Remove:
                RemovePointer(ReturnPointer());
                break;
            case MapInteractions.Mark:
                Vector3 tempMark = ReturnHitPoint();

                break;
            case MapInteractions.None:
                break;
            default:
                break;
        }

    }

    private void RemovePointer(Vector3 pos)
    {
        if(Pointers.Exists(vec => vec == pos))
            Pointers.Remove(pos);
    }

    private Vector3 ReturnPointer()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        if(Physics.Raycast(
            r, out var hit, 5f,
            LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable")))
        {
            if(hit.transform.tag == "MapPointer") return hit.transform.position;
        }

        return Vector3.zero;
    }

    private Vector3 ReturnHitPoint()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        
        if(Physics.Raycast(
            r, out var hit, 5f,
            LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable")))
        {
            if(hit.transform.tag == "Map") return hit.point;
        }

        return Vector3.zero;
    }

    void Update()
    {

    }

    public void CreatePoint()
    {

    }

    public void SetMapState(MapInteractions mi)
    {
        MapState = mi;
    }
}