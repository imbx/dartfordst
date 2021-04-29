using UnityEngine.InputSystem;
using UnityEngine;

public class Keybinds : MonoBehaviour {

    public PrimaryController controller;
    InputAction movementAction;
    InputAction lookAction;
    bool        mouseRightButtonPressed;
    
    void Start() {
        var map = new InputActionMap("DartfordStController");
        lookAction = map.AddAction("look", binding: "<Mouse>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15, y=15)");
        movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");
        movementAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        movementAction.Enable();
        lookAction.Enable();
    }


    void Update () {
        Vector2 vecMov = new Vector2(movementAction.ReadValue<Vector2>().y, movementAction.ReadValue<Vector2>().x);
        controller.Axis = vecMov;
        controller.CameraAxis = lookAction.ReadValue<Vector2>();
        controller.Mouse = mousePosition();

        controller.isEscapePressed = isEscapePressed();
        controller.isInputPressed = isActionPressed();
        controller.isLanternPressed = isLanternPressed();
    }

    bool isEscapePressed() {
        return Keyboard.current != null ? Keyboard.current.escapeKey.isPressed : false; 
    }

    bool isActionPressed () {
        return Keyboard.current != null ? Keyboard.current.spaceKey.isPressed : false; 
    }

    bool isLanternPressed () {
        return Keyboard.current != null ? Keyboard.current.qKey.isPressed : false; 
    }

    Vector2 mousePosition() {
        return Mouse.current.position.ReadValue();
    }
}