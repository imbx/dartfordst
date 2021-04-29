using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Audio;
public class GameController : MonoBehaviour{
    public static GameController current;
    public Camera mainCamera;
    public Database database;
    private Texture2D[] CursorTextures;
    public EntityData Player;
    public GameObject UI;

    public void SetCursor() {

    }

    public void ToggleCursor(bool isVisible = true) => 
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;

    public bool LoadResources () {
        return true;
    }

    private void Awake() {
        current = this;
        ToggleCursor(false);
        database = new Database();
        Player.CanMove = true;
        Player.CanLook = true;
    }

    public void DisableUI() {
        UI.SetActive(false);
    }

    public void EnableUI() {
        UI.SetActive(true);
    }

}