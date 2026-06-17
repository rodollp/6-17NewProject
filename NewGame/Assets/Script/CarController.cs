using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float turnSpeed = 120f;

    Rigidbody rb;

    float moveInput;
    float turnInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = 0;
        turnInput = 0;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed)
            moveInput = 1;

        if (Keyboard.current.sKey.isPressed)
            moveInput = -1;

        if (Keyboard.current.aKey.isPressed)
            turnInput = -1;

        if (Keyboard.current.dKey.isPressed)
            turnInput = 1;
    }

    private void FixedUpdate()
    {
        Vector3 move =
            transform.forward *
            moveInput *
            moveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Quaternion turn =
                Quaternion.Euler(
                    0,
                    turnInput * turnSpeed * Time.fixedDeltaTime,
                    0);

            rb.MoveRotation(rb.rotation * turn);
        }
    }
}