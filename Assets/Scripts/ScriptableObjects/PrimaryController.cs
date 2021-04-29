using UnityEngine;

[CreateAssetMenu]
public class PrimaryController : ScriptableObject{
    public Vector2 Axis = Vector2.zero;
    public Vector2 CameraAxis = Vector2.zero;
    public bool isInputPressed = false;
    public bool isEscapePressed = false;
    public bool isLanternPressed = false;
    public bool isBoostPressed = false;
    public Vector2 Mouse = Vector2.zero;

    // Temporal, no sabemos si añadir boost de velocidad, no encaja en el juego
}