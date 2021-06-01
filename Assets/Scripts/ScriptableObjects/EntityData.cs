using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject, ISerializationCallbackReceiver {
    public float Speed = 6f;
    public float RunSpeedMultiplier = 1.5f;
    public float VisionRange = 50f;
    public Vector3 CameraRotations = Vector3.zero;
    public bool CanMove = true;
    public bool CanLook = true;

    public bool IsBlockedByEvent = false;

    public Vector3 PlayerPosition = Vector3.zero;
    public bool isLoadingData = false;
    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize() { }

    public void ControlMovement(bool isTrue) {
        CanMove = isTrue;
        CanLook = isTrue;
    }
}