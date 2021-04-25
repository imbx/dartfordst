using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Presets;
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
    public GameController m_GameController;
    public Preset m_startPosition;
    private Vector3 m_ROrigin;

    public GameObject Lantern;

    [SerializeField] private float _isPressedCd = 0f;
    [SerializeField] private bool isLanternActive;
    [SerializeField] private float lanternInputCd = 0f;

    void Awake()
    {
        
    }
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

        RaycastHit hit;
        m_ROrigin = m_GameController.mainCamera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        if(_isPressedCd > 0) _isPressedCd -= Time.deltaTime;
        if(Physics.Raycast(m_ROrigin, m_GameController.mainCamera.transform.forward, out hit, m_CVars.VisionRange, LayerMask.GetMask("Interactuable"))){
            Debug.Log(hit.collider.name);
            if(_isPressedCd <= 0 && m_PlayerMovement.isInputPressed) {
                if(hit.collider.GetComponent<InteractBase>()){
                    hit.collider.GetComponent<InteractBase>().Execute();
                    _isPressedCd = 0.75f;
                }

                if(hit.collider.GetComponent<LightboxSwitches>()) {
                    hit.collider.GetComponent<LightboxSwitches>().ToggleSwitch();
                    _isPressedCd = 0.75f;
                }
            }

            //Transform hitObject = hit.collider.GetComponent<Transform>();
        } 

        if(m_CVars.CanLook)
        {
            Vector2 mouseAxis = m_PlayerMovement.CameraAxis;
            m_Yaw = m_Yaw + mouseAxis .x * m_CConfig.YawSpeed * Time.deltaTime * (m_CConfig.InvertX ? -1 : 1);
            m_Pitch = m_Pitch + mouseAxis.y * m_CConfig.PitchSpeed * Time.deltaTime * (m_CConfig.InvertY ? -1 : 1);
            m_Pitch = Mathf.Clamp(m_Pitch, m_CConfig.MinPitch, m_CConfig.MaxPitch);

            transform.rotation = Quaternion.Euler(0, m_Yaw, 0);
            m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0, 0);


        }

        if(lanternInputCd > 0f) lanternInputCd -= Time.deltaTime;

        if(m_PlayerMovement.isLanternPressed && !isLanternActive && lanternInputCd <= 0f) {
            isLanternActive = true;
            lanternInputCd = 1f;
            // GameController.current.lanternActive = true;
        }
        if(m_PlayerMovement.isLanternPressed && isLanternActive && lanternInputCd <= 0f){
            isLanternActive = false;
            lanternInputCd = 1f;
        }

        Lantern.SetActive(isLanternActive);
        

        if(m_CVars.CanMove)
        {
            Vector3 l_Forward = transform.forward;
            Vector3 l_Right = transform.right;
            Vector3 l_Movement = Vector3.zero;
            Vector2 l_Axis = m_PlayerMovement.Axis;

            l_Movement = l_Forward * l_Axis.x;
            l_Movement += l_Right * l_Axis.y;

            l_Movement.Normalize();

            l_Movement = l_Movement * m_CVars.Speed * Time.deltaTime;

            m_characterController.Move(l_Movement);
        }
    }

    #region Reset
    public void Reset()
    {
        m_startPosition.ApplyTo(transform);
        m_CVars.OnAfterDeserialize();
    }
    #endregion
}
