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