using UnityEngine;
using System.Collections.Generic;
using BoxScripts;
using UnityEditor.Events;

public class Map : InteractBase {
    private List<GameObject> Marker1Pointers;
    private List<GameObject> Marker2Pointers;
    private GameObject Marker3;
    private PinSelect pinSelect = PinSelect.None;
    [SerializeField] private PrimaryController controller;

    public GameObject PinPrefab;

    public Material Red;
    public Material Blue;

    public LineRenderer RedLine;
    public LineRenderer BlueLine;


    private bool hasGivenPage = false;
    

    private void OnEnable() {
        Marker1Pointers = new List<GameObject>();
        Marker2Pointers = new List<GameObject>();
    }

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);

        if(pinSelect != PinSelect.None && isLeftAction)
        {
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
            GameObject go = Instantiate(PinPrefab);
            MapPin mapPin = go.GetComponent<MapPin>();
            mapPin.SetPin(transform, pinSelect == PinSelect.Red ? Red : Blue);
            go.transform.position = pos;
            if(pinSelect == PinSelect.Red)
            {
                Marker1Pointers.Add(go);
                return;
            }
            Marker2Pointers.Add(go);
        }
    }

    public void RemovePin(GameObject ioa)
    {

        if(Marker1Pointers.Exists(vec => vec == ioa))
            Marker1Pointers.Remove(ioa);
        if(Marker2Pointers.Exists(vec => vec == ioa))
            Marker2Pointers.Remove(ioa);

        Destroy(ioa);
    }

    private Transform ReturnPointer()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        if(Physics.Raycast(
            r, out var hit, 5f,
            LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable")))
        {
            if(hit.transform.tag == "MapPin") return hit.transform;
        }

        return null;
    }

    private Vector3 ReturnHitPoint()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        
        if(Physics.Raycast(
            r, out var hit, Mathf.Infinity,
            LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable")))
        {
            if(hit.transform.tag == "Map") return hit.point;
        }

        return Vector3.zero;
    }

    void Update()
    {
        if(Marker1Pointers.Count > 1)
        {
            if(!RedLine.enabled) RedLine.enabled = true;
            SetLineRenderer(Marker1Pointers, RedLine);
        } else if(RedLine.enabled) RedLine.enabled = false;
        if(Marker2Pointers.Count > 1)
        {
            if(!BlueLine.enabled) BlueLine.enabled = true;
            SetLineRenderer(Marker2Pointers, BlueLine);
        } else if(BlueLine.enabled) BlueLine.enabled = false;


        if(controller.isEscapePressed && !hasGivenPage) {
            hasGivenPage = true;
            GameController.current.database.AddNotePage(12432, "Paris");
            this.OnEnd();
        }
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