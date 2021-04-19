using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class CameraSettings : ScriptableObject {
    public float MinPitch = -35f;
    public float MaxPitch = 105f;
    public float YawSpeed = 90f;
    public float PitchSpeed = 45f;
    public bool InvertY = true;
    public bool InvertX = false;
}

public class CharController : MonoBehaviour
{
    [Header("Camera Movement")]
    public CameraSettings m_CConfig;
    public Transform m_PitchController;

    private float m_Yaw = 0f;
    private float m_Pitch = 0f;

    [Header("Movement")]
    private CharacterController m_characterController;
    public float m_Speed = 6f;
    public float m_RunSpeedMultiplier = 1.5f;

    [Header("Stats")]
    public GameController m_GameController;
    public Preset m_startPosition;

    [SerializeField]
    public CharacterVariables m_CharacterVars;

    //m_AngleLocked

    void Start()
    {
        m_startPosition.ApplyTo(transform);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        m_characterController = GetComponent<CharacterController>();
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

        l_Movement = l_Movement * m_Speed * ((Input.GetButton("Fire1")) ? m_RunSpeedMultiplier : 1f) * Time.deltaTime;

        m_characterController.Move(l_Movement);
    }

    #region Reset
    public void Reset()
    {
        m_startPosition.ApplyTo(transform);
        m_CharacterVars.OnAfterDeserialize();
    }
    #endregion
}
