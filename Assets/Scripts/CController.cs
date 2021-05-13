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
    public GameControllerObject gcObject;
    private Vector3 m_ROrigin;
    [SerializeField] private float _isPressedCd = 0f;
    [SerializeField] private float InteractRange = 2f;
    [SerializeField] private float l_gravity = 9.8f;

    void Awake()
    {
        
    }
    void Start()
    {
        //m_startPosition.ApplyTo(transform);
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
        
        m_ROrigin = gcObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 direction = gcObject.camera.transform.forward;
        if(!m_CVars.CanLook) {
            Ray mouseHit = Camera.main.ScreenPointToRay(m_PlayerMovement.Mouse);
            m_ROrigin = Camera.main.ScreenToWorldPoint(m_PlayerMovement.Mouse);
            direction = mouseHit.direction;
        }

        if(_isPressedCd > 0) _isPressedCd -= Time.deltaTime;
        if(Physics.Raycast(m_ROrigin, direction, out hit, m_CVars.VisionRange, LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable"))){
            bool leftButton = m_PlayerMovement.isInputPressed;
            if(
                (!m_CVars.CanLook && hit.collider.GetComponent<InteractBase>()) ||
                (hit.collider.GetComponent<InteractBase>() && Mathf.Abs((hit.transform.position - transform.position).magnitude) < InteractRange)
            )
            {
                if( gcObject.state != BoxScripts.GameState.TARGETING &&
                    gcObject.state != BoxScripts.GameState.INTERACTING &&
                    gcObject.state != BoxScripts.GameState.ENDINTERACTING &&
                    gcObject.state != BoxScripts.GameState.LOOKITEM &&
                    gcObject.state != BoxScripts.GameState.ENDLOOKITEM)
                    {
                        switch(hit.collider.tag)
                        {
                            case "Picture":
                                gcObject.ChangeState(BoxScripts.GameState.TARGETINGPICTURE);
                                GameController.current.SetCursor();
                                break;
                            case "Locked":
                                // SHOULD CHANGE CURSOR
                                Debug.Log("Is locked, shouldnt do anything");
                                break;
                            default:
                                gcObject.ChangeState(BoxScripts.GameState.TARGETING);
                                break;
                        }
                    }
                if(hit.collider.tag == "Item" || gcObject.state != BoxScripts.GameState.LOOKITEM)
                if(_isPressedCd <= 0 && (leftButton || m_PlayerMovement.isInput2Pressed)) {
                    _isPressedCd = 0.5f;
                    hit.collider.GetComponent<InteractBase>().Execute(leftButton);
                }
            }
        }
        else
        {
            if(gcObject.state == BoxScripts.GameState.TARGETING) gcObject.ChangeState(BoxScripts.GameState.PLAYING);
        }

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
