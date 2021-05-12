using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using BoxScripts;

public class GameController : MonoBehaviour{
    public static GameController current;
    [SerializeField] private GameControllerObject gameCObject;
    public UIController ui;
    public Database database;
    private Texture2D[] CursorTextures;
    public EntityData Player;
    public PrimaryController PlayerControls;
    public Transform Hand;
    [SerializeField] private VolumeProfile postFxVol;
    [SerializeField] private ClampedFloatParameter contrast;
    [SerializeField] private float blurSpeed = 2f;

    private float keyPressCooldown = 0.75f;


    public void SetCursor() {

    }

    public void ToggleCursor(bool isVisible = true) => 
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;

    public bool LoadResources () {
        return true;
    }

    private void Awake() {
        current = this;
        Debug.Log("Loading Database");
        database = new Database();
        Debug.Log("-- DB LOADED --");
        gameCObject.camera = Camera.main;
        gameCObject.ChangeState(GameState.LOADGAME);
        
        ui.HideUI();
    }

    public void DisableUI() {
        ui.gameObject.SetActive(false);
    }

    public void EnableUI() {
        ui.gameObject.SetActive(true);
    }

    private bool ApplyPostEffect()
    {
        if(contrast.value < 99f)
        {
            contrast.value += blurSpeed;
            return false;
        } else contrast.value = 100f;
        return true;
    }

    public void DisablePostEffect()
    {
        contrast.value = 0f;
    }

    public void ChangeState(GameState gameState)
    {
        gameCObject.ChangeState(gameState);
    }

    private void Update() {
        keyPressCooldown -= Time.deltaTime;
        switch(gameCObject.state)
        {
            case GameState.TITLESCREEN:
                break;
            case GameState.LOADGAME:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    ToggleCursor(false);
                    Player.CanMove = true;
                    Player.CanLook = true;

                    if (!postFxVol.TryGet<ColorAdjustments>(out var ca))
                            ca = postFxVol.Add<ColorAdjustments>(false);

                    contrast = ca.contrast;
                    DisablePostEffect();
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;
            case GameState.PLAYING:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    ui.HideUI();
                }
                
                if(PlayerControls.isTabPressed && keyPressCooldown <= 0f){
                    keyPressCooldown = 0.75f;

                    gameCObject.ChangeState(GameState.OPENNOTEBOOK);
                }
                // HERE KEYBINDS FOR NOTEBOOK, UI AND SO

                break;

            case GameState.TARGETING:
                if(gameCObject.justChangedState)
                {
                    ui.SetBasicInteract(true);
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
                    ui.SetPicturesInteract(true);
                    gameCObject.justChangedState = false;
                }

                if(PlayerControls.isTabPressed && keyPressCooldown <= 0f){
                    keyPressCooldown = 0.75f;

                    gameCObject.ChangeState(GameState.OPENNOTEBOOK);
                }
                break;
            case GameState.MOVINGPICTURE:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                }
                break;
            case GameState.INTERACTING:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    ToggleCursor();
                    Player.CanMove = false;
                    Player.CanLook = false;
                }
                if(gameCObject.requireFocus) ApplyPostEffect();
                break;
            case GameState.ENDINTERACTING:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    DisablePostEffect();
                    ToggleCursor(false);
                    Player.CanMove = true;
                    Player.CanLook = true;
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;

            case GameState.LOOKITEM:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    ToggleCursor();
                    Player.CanMove = false;
                    Player.CanLook = false;
                }
                ApplyPostEffect();
                break;
            case GameState.ENDLOOKITEM:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    DisablePostEffect();
                    ToggleCursor(false);
                    Player.CanMove = true;
                    Player.CanLook = true;
                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;
            case GameState.OPENNOTEBOOK:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    ToggleCursor();
                    Player.CanMove = false;
                    Player.CanLook = false;
                }
                if(gameCObject.requireFocus && ApplyPostEffect())
                    ui.SetNotebookActive(true);
                else ui.SetNotebookActive(true);

                if(PlayerControls.isTabPressed && keyPressCooldown <= 0f){
                    keyPressCooldown = 0.75f;
                    gameCObject.ChangeState(GameState.CLOSENOTEBOOK);
                }
                break;
            case GameState.CLOSENOTEBOOK:
                if(gameCObject.justChangedState)
                {
                    gameCObject.justChangedState = false;
                    DisablePostEffect();
                    ui.SetNotebookActive(false);
                    ToggleCursor(false);
                    Player.CanMove = true;
                    Player.CanLook = true;

                }
                gameCObject.ChangeState(GameState.PLAYING);
                break;
            case GameState.ENDGAME:
                break;
            default:
                break;   
        }
    }
}