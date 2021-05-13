using UnityEngine;

public class Door : InteractBase {
    public bool hasRequirement = false;
    public int reqID = -1;
    public float rotationSpeed = 2f;
    public float targetAngle = 90f; // CALCULATED OVER Y AXIS



    private bool isDoorOpen = false;

    private Vector3 BaseRotation;
    private float eulerYAngle = 0;

    private float timer = 0f;



    void OnEnable() { 
        BaseRotation = transform.localEulerAngles;
        eulerYAngle = BaseRotation.y;
    }


    public override void Execute(bool isLeftAction = true)
    {
        if(hasRequirement)
        {
            if(!GameController.current.database.GetProgressionState(reqID))
                return;
        }
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        timer = 0;
        eulerYAngle = transform.localEulerAngles.y;
        isDoorOpen = !isDoorOpen;
    }

    void Update()
    {

        if(hasRequirement)
        {
            if(!GameController.current.database.GetProgressionState(reqID))
                transform.tag = "Untagged";

        }

        if(timer < 1f)
            timer += Time.deltaTime * rotationSpeed;

        transform.localEulerAngles = new Vector3(BaseRotation.x, Mathf.LerpAngle(eulerYAngle, BaseRotation.y + (isDoorOpen ? targetAngle : 0), timer), BaseRotation.z);
    }
}