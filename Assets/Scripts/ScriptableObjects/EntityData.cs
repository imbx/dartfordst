using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class EntityData : ScriptableObject, ISerializationCallbackReceiver {
    public float Speed = 6f;
    public float RunSpeedMultiplier = 1.5f;
    public float VisionRange = 50f;
    public bool CanMove = true;
    public bool CanLook = true;
    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize() { }
}