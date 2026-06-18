using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public float MoveInput {  get; private set; }
    public float SteerInput { get; private set; }

    public bool BrakeInput { get; private set; }

    private void Update()
    {
        MoveInput = 0f;
        SteerInput = 0f;
        BrakeInput = false;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed)
            MoveInput = 1f;

        if (Keyboard.current.sKey.isPressed)
            MoveInput = -1f;

        if (Keyboard.current.aKey.isPressed)
            SteerInput = -1f;

        if (Keyboard.current.dKey.isPressed)
            SteerInput = 1f;

        BrakeInput = Keyboard.current.spaceKey.isPressed;
    }

}


