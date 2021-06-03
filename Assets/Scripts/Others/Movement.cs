using UnityEngine;
using BoxScripts;

public class Movement : MonoBehaviour {
    private TransformData from;
    private TransformData target;
    private bool hasToRotate = false;
    private float Speed = 2f;
    [HideInInspector] public bool hasParameters = false;
    private float timer = 0f;
    [HideInInspector] public bool HasToDestroy = false;
    [SerializeField] public bool isAtDestination = true;

    void Update()
    {
        if(!isAtDestination)
        { 
            if(timer < 1f)
                timer += Time.deltaTime * Speed;
            else {
                isAtDestination = true;
                //hasParameters = false;
                if(HasToDestroy) Destroy(this);
            }
            if(hasToRotate) transform.eulerAngles = from.LerpAngle(target, timer);
            transform.position = from.LerpDistance(target, timer);
        }
    }

    public void SetConfig(float speed = 2f, bool rotate = false)
    {
        Speed = speed;
        hasToRotate = rotate;
    }

    public void SetParameters(Transform target, Transform from = null)
    {
        this.target = new TransformData(target);
        this.from = from ? new TransformData(from) : new TransformData(transform);
        isAtDestination = false;
        hasParameters = true;
        timer = 0f;
    }

    public void SetParameters(TransformData target, TransformData from = null)
    {
        this.target = target;
        this.from = from != null ? from : new TransformData(transform);
        isAtDestination = false;
        hasParameters = true;
        timer = 0f;
    }

    public void Invert()
    {
        timer = 0;
        TransformData tempTransform = from;
        from = target;
        target = tempTransform;
        isAtDestination = false;
        hasParameters = true;
    }
}