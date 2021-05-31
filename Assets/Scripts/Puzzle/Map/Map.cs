using UnityEngine;
using System.Collections.Generic;
using BoxScripts;

public class Map : InteractBase {
    private GameObject Marker3;
    private PinSelect pinSelect = PinSelect.None;

    public GameObject PinPrefab;

    public Material Red;
    public Material Blue;

    public LineRenderer RedLine;
    public LineRenderer BlueLine;


    private bool hasGivenPage = false;

    public MapPuzzle mapPuzzle;


    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);

        if(pinSelect != PinSelect.None && isLeftAction)
        {
            Debug.Log("[Map] Setting pin");
            SetPin();
        }
        
        if(!isLeftAction)
        {
            Transform temp = ReturnPointer();
            // RemovePin();
            Debug.Log("Pin name is " + temp.name);
        }

    }

    public void SetPin()
    {
        Vector3 pos = ReturnHitPoint();
        if(pos != Vector3.zero)
        {
            Debug.Log("[Map] Instantiating prefab");
            GameObject go = Instantiate(PinPrefab);
            MapPin mapPin = go.GetComponent<MapPin>();
            mapPin.SetPin(transform, pinSelect == PinSelect.Red ? Red : Blue);
            go.transform.position = pos;
            if(pinSelect == PinSelect.Red)
            {
                mapPuzzle.RedPinList.Add(go);
                return;
            }
            mapPuzzle.BluePinList.Add(go);
        }
    }

    public void RemovePin(GameObject ioa)
    {

        if(mapPuzzle.RedPinList.Exists(vec => vec == ioa))
            mapPuzzle.RedPinList.Remove(ioa);
        if(mapPuzzle.BluePinList.Exists(vec => vec == ioa))
            mapPuzzle.BluePinList.Remove(ioa);

        Destroy(ioa);
    }

    private Transform ReturnPointer()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)mapPuzzle.controller.Mouse);
        if(Physics.Raycast(
            r, out var hit, 5f,
            LayerMask.GetMask("Focus")))
        {
            if(hit.transform.tag == "MapPin") return hit.transform;
        }

        return null;
    }

    private Vector3 ReturnHitPoint()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)mapPuzzle.controller.Mouse);
        Debug.Log("[Map] Returning HitPoint");
        if(Physics.Raycast(
            r, out var hit, Mathf.Infinity,
            LayerMask.GetMask("Focus")))
        {

            Debug.Log(hit.point);
            if(hit.transform.tag == "Map") return hit.point;
        }

        return Vector3.zero;
    }

    void Update()
    {
        
        if(mapPuzzle.RedPinList.Count > 1)
        {
            if(!RedLine.enabled) RedLine.enabled = true;
            SetLineRenderer(mapPuzzle.RedPinList, RedLine);
        } else if(RedLine.enabled) RedLine.enabled = false;
        if(mapPuzzle.BluePinList.Count > 1)
        {
            if(!BlueLine.enabled) BlueLine.enabled = true;
            SetLineRenderer(mapPuzzle.BluePinList, BlueLine);
        } else if(BlueLine.enabled) BlueLine.enabled = false;
    }

    void SetLineRenderer(List<GameObject> list, LineRenderer lr)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach(GameObject go in list)
        {
            positions.Add(go.GetComponent<MapPin>().lineStart.position);
        }
        lr.positionCount = list.Count;
        lr.SetPositions(positions.ToArray());
    }

    public void SetPin(PinSelect ps)
    {
        pinSelect = ps;
    }
}