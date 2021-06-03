using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;

public class CController : MonoBehaviour
{
    [Header("Camera Movement")]
    public CameraSettings m_CConfig;
    public Transform m_PitchController;

    private float m_Yaw = 0f;
    private float m_Pitch = 0f;

    [Header("Movement")]
    private CharacterController m_characterController;
    public EntityData m_CVars;
    public PrimaryController m_PlayerMovement;

    [Header("Stats")]
    [SerializeField] private float l_gravity = 9.8f;


    private FMODUnity.StudioEventEmitter eventEmiterRef;

    private void Awake()
    {
        eventEmiterRef = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void OnEnable()
    {
        m_characterController = GetComponent<CharacterController>();
        if(m_CVars.isLoadingData) 
        {
            transform.position = m_CVars.PlayerPosition;
            transform.rotation = Quaternion.Euler(0, m_Yaw, 0);
            m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0, 0);
        }
    }

    void Update()
    {
        // FindObjectOfType<AudioManager>().Play("player_steps");

        if(m_CVars.CanLook) CameraMovement();
        if(m_CVars.CanMove) Movement();
        
    }
    #region UpdateFunctions
    private void CameraMovement()
    {
        Vector2 mouseAxis = m_PlayerMovement.CameraAxis;
        m_Yaw = m_Yaw + mouseAxis .x * m_CConfig.YawSpeed * Time.deltaTime * (m_CConfig.InvertX ? -1 : 1);
        m_Pitch = m_Pitch + mouseAxis.y * m_CConfig.PitchSpeed * Time.deltaTime * (m_CConfig.InvertY ? -1 : 1);
        m_Pitch = Mathf.Clamp(m_Pitch, m_CConfig.MinPitch, m_CConfig.MaxPitch);

        transform.rotation = Quaternion.Euler(0, m_Yaw, 0);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0, 0);

        m_CVars.CameraRotations = new Vector3(m_Pitch, 0, m_Yaw);
    }

    private void Movement()
    {
        Vector3 l_Forward = transform.forward;
        Vector3 l_Right = transform.right;
        Vector3 l_Movement = Vector3.zero;
        Vector2 l_Axis = m_PlayerMovement.Axis;

        l_Movement = l_Forward * l_Axis.x;
        l_Movement += l_Right * l_Axis.y;
        l_Movement += transform.up * -1 * l_gravity * Time.deltaTime;
        l_Movement.Normalize();

        l_Movement = l_Movement * m_CVars.Speed * Time.deltaTime;

        m_characterController.Move(l_Movement);
        m_CVars.PlayerPosition = transform.position;
    }
    #endregion

    #region Reset
    public void Reset()
    {
        //m_startPosition.ApplyTo(transform);
        m_CVars.OnAfterDeserialize();
    }
    #endregion
}
