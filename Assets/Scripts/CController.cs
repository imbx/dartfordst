using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Audio;
<<<<<<< HEAD
public class CController : MonoBehaviour
{
=======

[CreateAssetMenu]
public class CameraSettings : ScriptableObject {
    public float MinPitch = -35f;
    public float MaxPitch = 105f;
    public float YawSpeed = 90f;
    public float PitchSpeed = 45f;
    public bool InvertY = true;
    public bool InvertX = false;
}
[CreateAssetMenu]
public class NPCData : ScriptableObject, ISerializationCallbackReceiver {
    public float Speed = 6f;
    public float RunSpeedMultiplier = 1.5f;
    public float VisionRange = 50f;
    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize() { }
}

public class GameController : MonoBehaviour{
    public Camera mainCamera;

}

public class CController : MonoBehaviour
{


>>>>>>> 8f78af3cf7a33cd3920d32e158f3b9ecf6331316
    [Header("Camera Movement")]
    public CameraSettings m_CConfig;
    public Transform m_PitchController;

    private float m_Yaw = 0f;
    private float m_Pitch = 0f;

    [Header("Movement")]
    private CharacterController m_characterController;
<<<<<<< HEAD
    public EntityData m_CVars;
=======
    public NPCData m_CVars;
>>>>>>> 8f78af3cf7a33cd3920d32e158f3b9ecf6331316

    [Header("Stats")]
    public GameController m_GameController;
    public Preset m_startPosition;

    private Vector3 m_ROrigin;
    private LineRenderer m_Line;

    void Awake() {
        
    }
    void Start()
    {
        m_startPosition.ApplyTo(transform);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        m_ROrigin = m_GameController.mainCamera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        m_characterController = GetComponent<CharacterController>();

        #region UNITY_EDITOR
        if(!GetComponent<LineRenderer>())
            m_Line = this.gameObject.AddComponent<LineRenderer>();
        else
            m_Line = GetComponent<LineRenderer>();
        #endregion
    }

    void Update()
    {
        // FindObjectOfType<AudioManager>().Play("player_steps");
        #region UNITY_EDITOR
        /*if (Input.GetKeyDown(m_DebugLockAngleKeyCode)) m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }*/
        #endregion

        RaycastHit hit;

        #region UNITY_EDITOR
        m_Line.SetPosition(0, this.transform.position);
        #endregion

        if(Physics.Raycast(m_ROrigin, m_GameController.mainCamera.transform.forward, out hit, m_CVars.VisionRange, ~(1<<gameObject.layer))){
            Debug.Log(hit.collider.name);

            #region UNITY_EDITOR
            m_Line.SetPosition(1, hit.point);
            #endregion

            //Transform hitObject = hit.collider.GetComponent<Transform>();
        } 
        #region UNITY_EDITOR
        else {
            m_Line.SetPosition(1, m_ROrigin + (m_GameController.mainCamera.transform.forward * m_CVars.VisionRange));
        }
        #endregion

        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        m_Yaw = m_Yaw + mouseAxis.x * m_CConfig.YawSpeed * Time.deltaTime * (m_CConfig.InvertX ? -1 : 1);
        m_Pitch = m_Pitch + mouseAxis.y * m_CConfig.PitchSpeed * Time.deltaTime * (m_CConfig.InvertY ? -1 : 1);
        m_Pitch = Mathf.Clamp(m_Pitch, m_CConfig.MinPitch, m_CConfig.MaxPitch);

        transform.rotation = Quaternion.Euler(0, m_Yaw, 0);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0, 0);

        Vector3 l_Forward = transform.forward;
        Vector3 l_Right = transform.right;
        Vector3 l_Movement = Vector3.zero;
        Vector2 l_Axis = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

        l_Movement = l_Forward * l_Axis.x;
        l_Movement += l_Right * l_Axis.y;

        l_Movement.Normalize();

        l_Movement = l_Movement * m_CVars.Speed * ((Input.GetButton("Fire1")) ? m_CVars.RunSpeedMultiplier : 1f) * Time.deltaTime;

        m_characterController.Move(l_Movement);
    }

    #region Reset
    public void Reset()
    {
        m_startPosition.ApplyTo(transform);
        m_CVars.OnAfterDeserialize();
    }
    #endregion
<<<<<<< HEAD
}
=======
}
>>>>>>> 8f78af3cf7a33cd3920d32e158f3b9ecf6331316
