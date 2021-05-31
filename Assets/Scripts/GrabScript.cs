using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{

    public GameObject m_Player;
    public Camera m_Camera;
    // public KeyCode m_AttachedKey = KeyCode.E;
    public PrimaryController m_PlayerMovement;
    public Transform m_AttachedObjectTransform;
    GameObject m_ObjectAttached;
    bool m_AttachingObject = false;
    bool m_AttachedObject = false;
    public float m_MaxDistanceToAttachObject = 500.0f;
    public LayerMask m_AttachObjectLayerMask;
    float m_AttachingObjectCurrentTime = 0.0f;
    public float m_AttachedObjectTime = 1.0f;

    private FMODUnity.StudioEventEmitter eventEmiterRef;

    private void Awake()
    {
        eventEmiterRef = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_PlayerMovement.isInputPressed && m_ObjectAttached == null)
        {
            TryAttachObject();
        }
        if (m_ObjectAttached != null)
        {
            UpdateAttachObject();
        }

        

    }
    void UpdateAttachObject()
    {
        m_AttachingObjectCurrentTime += Time.deltaTime;
        if (m_AttachingObject)
        {
            float l_Pct = Mathf.Min(m_AttachingObjectCurrentTime / m_AttachedObjectTime, 1.0f);
            m_ObjectAttached.transform.position = Vector3.Lerp(m_ObjectAttached.transform.position, m_AttachedObjectTransform.position, l_Pct);
            m_ObjectAttached.transform.rotation = Quaternion.Lerp(m_ObjectAttached.transform.rotation, m_AttachedObjectTransform.rotation, l_Pct);
            if (l_Pct == 1.0f)
            {
                m_AttachingObject = false;
                m_AttachedObject = true;
                m_ObjectAttached.transform.SetParent(m_AttachedObjectTransform);
            }
        }

        else if (m_AttachedObject)
        {
            if (m_PlayerMovement.isInputPressed)
            {
                ThrowAttachObject(0.0f);
            }
        }
    }

    void ThrowAttachObject(float Force)
    {
        m_ObjectAttached.GetComponent<Collider>().enabled = true;
        m_ObjectAttached.transform.SetParent(null);
        Rigidbody l_Rigidbody = m_ObjectAttached.GetComponent<Rigidbody>();
        l_Rigidbody.isKinematic = false;
        l_Rigidbody.AddForce(m_AttachedObjectTransform.up * Force);
        m_AttachedObject = false;
        m_ObjectAttached = null;

    }

    void TryAttachObject()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxDistanceToAttachObject, m_AttachObjectLayerMask))
        {
            if (l_RaycastHit.collider.tag == "Attachable")
            {
                AttachObject(l_RaycastHit.collider);
            }
            if (l_RaycastHit.collider.tag == "Cajones")
            {
                l_RaycastHit.collider.transform.position += new Vector3(0f, 0f, 0.6f);
            }

        }
    }

    void AttachObject(Collider _Collider)
    {
        m_AttachingObject = true;
        m_ObjectAttached = _Collider.gameObject;
        _Collider.enabled = false;
        _Collider.GetComponent<Rigidbody>().isKinematic = true;
        m_AttachingObjectCurrentTime = 0.0f;
    }
}
