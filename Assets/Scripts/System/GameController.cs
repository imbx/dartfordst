using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;
using BoxScripts;

public class GameController : MonoBehaviour{
    public static GameController current;
    public GameControllerObject gameCObject;
    public UIController ui;
    public Database database;
    private Texture2D[] CursorTextures;
    public EntityData Player;
    public PrimaryController PlayerControls;
    public Transform Hand;

    public TextManager textManager;
    //[SerializeField] private VolumeProfile postFxVol;
    //[SerializeField] private FloatParameter contrast;
    [SerializeField] private float blurSpeed = 2f;

    private float keyPressCooldown = 0.75f;
    private int counter = 1;
    private List<InteractBase> AllInteractions;

    public MusicControler music;

    public Texture2D CircleCursor;

    private void Awake() {
        current = this;
        Debug.Log("Loading Database");
        database = new Database(Player);
        Debug.Log("-- DB LOADED --");
        gameCObject.camera = Camera.main;
        AllInteractions = new List<InteractBase>();
        gameCObject.ChangeState(GameState.LOADGAME);
        ui.HideUI();

        Vector2 cursorOffset = new Vector2(CircleCursor.width/2, CircleCursor.height/2);
        Cursor.SetCursor(CircleCursor, cursorOffset, CursorMode.Auto);
    }

    private void Update() {
        keyPressCooldown -= Time.deltaTime;
        UpdateTargetUI();
        switch(gameCObject.state)
        {
            case GameState.TITLESCREEN:
                break;
            case GameState.LOADGAME:
                if(gameCObject.justChangedState)
                {
                    StateChange(false);
                    LoadInteractions();
                    /*if (!postFxVol.TryGet<ColorAdjustments>(out var ca))
                            ca = postFxVol.Add<ColorAdjustments>(false);

                    contrast = ca.postExposure;*/

                    DisablePostEffect();
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;
            case GameState.PLAYING:
                if(gameCObject.justChangedState)
                {
                    DisablePostEffect();
                    StateChange(false);
                    ui.HideUI();
                }
                
                NotebookVisibility();

                if(PlayerControls.isInput2Pressed && keyPressCooldown <= 0f){
                    keyPressCooldown = 0.75f;

                    textManager.SpawnThought(counter);
                    counter++;
                }
                // HERE KEYBINDS FOR NOTEBOOK, UI AND SO

                break;

            case GameState.MOVINGCAMERA:
                if(gameCObject.justChangedState)
                {
                    CameraChange(false);
                    ui.HideUI();
                }
                break;

            case GameState.TARGETING:
                if(gameCObject.justChangedState)
                {
                    //ui.SetBasicInteract(true);
                    Debug.Log("Should enable UI");
                    gameCObject.justChangedState = false;
                }

                if(PlayerControls.isTabPressed && keyPressCooldown <= 0f){
                    keyPressCooldown = 0.75f;

                    gameCObject.ChangeState(GameState.OPENNOTEBOOK);
                }
                break;
            case GameState.TARGETINGPICTURE:
                if(gameCObject.justChangedState)
                {
                    Debug.Log("Should enable pic UI");
                    //ui.SetPicturesInteract(true);
                    gameCObject.justChangedState = false;
                }

                NotebookVisibility(true);
                break;
            case GameState.MOVINGPICTURE:
                if(gameCObject.justChangedState) gameCObject.justChangedState = false;
                break;

            case GameState.INTERACTING:
                if(gameCObject.justChangedState) StateChange(true);
                if(gameCObject.requireFocus) ApplyPostEffect();
                break;

            case GameState.ENDINTERACTING:
                if(gameCObject.justChangedState)
                {
                    DisablePostEffect();
                    StateChange(false);
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;

            case GameState.LOOKITEM:
                if(gameCObject.justChangedState) StateChange(true);
                ApplyPostEffect();
                break;

            case GameState.ENDLOOKITEM:
                if(gameCObject.justChangedState)
                {
                    DisablePostEffect();
                    StateChange(false);
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;

            case GameState.OPENNOTEBOOK:
                if(gameCObject.justChangedState) StateChange(true);
                if(gameCObject.requireFocus && ApplyPostEffect())
                    ui.SetNotebookActive(true);
                else ui.SetNotebookActive(true);
                NotebookVisibility(false);
                break;

            case GameState.CLOSENOTEBOOK:
                if(gameCObject.justChangedState)
                {
                    StateChange(true);
                    DisablePostEffect();
                    ui.SetNotebookActive(false);
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;
            case GameState.ENDGAME:
                break;
            default:
                break;   
        }
    }

    public void SetCursor() {
    
    }

    public void ToggleCursor(bool isVisible = true) => 
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;

    public bool LoadResources () {
        return true;
    }


    public void UpdateTargetUI()
    {
        ui.TargetUI(gameCObject.playerTargetTag);
    }

    public void SubscribeInteraction(InteractBase ib)
    {
        AllInteractions.Add(ib);
    }

    private void LoadInteractions()
    {
        foreach(InteractBase ib in AllInteractions)
        {
            ib.OnLoad();
        }
    }

    public void MoveCamera(Vector3 destination, Vector3 modifier, Vector3 rotation)
    {
        gameCObject.camera.GetComponent<CameraMovement>().PrepareMovement(destination, modifier, rotation);
    }

    public void RevertCamera()
    {
        gameCObject.camera.GetComponent<CameraMovement>().RevertCamera();
    }

    public void DisableUI() {
        //ui.gameObject.SetActive(false);
    }

    public void EnableUI() {
        //ui.gameObject.SetActive(true);
    }

    private bool ApplyPostEffect()
    {
        /*if(contrast.value > -1f)
        {
            contrast.value -= blurSpeed;
            return false;
        } else contrast.value = -1.15f;*/
        return true;
    }

    public void DisablePostEffect()
    {
        // contrast.value = 0f;
    }

    public void ChangeState(GameState gameState)
    {
        gameCObject.ChangeState(gameState);
    }

    private void StateChange(bool hasCursor)
    {
        gameCObject.justChangedState = false;
        ToggleCursor(hasCursor);
        Player.CanMove = !hasCursor;
        Player.CanLook = !hasCursor;
    }
    private void CameraChange(bool hasEnded)
    {
        gameCObject.justChangedState = false;
        ToggleCursor(hasEnded);
        Player.CanMove = hasEnded;
        Player.CanLook = hasEnded;
    }

    private void NotebookVisibility(bool show = true)
    {
        if(PlayerControls.isTabPressed && keyPressCooldown <= 0f){
            keyPressCooldown = 0.75f;
            gameCObject.ChangeState(show ? GameState.OPENNOTEBOOK : GameState.CLOSENOTEBOOK);
        }
    }
}