using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("입력")]
    [SerializeField] InputHandler input;

    [Header("주행")]
    [SerializeField] float maxForwardSpeed = 40f;
    [SerializeField] float maxReverseSpeed = 15f;
    [SerializeField] float acceleration = 25f;
    [SerializeField] float brakePower = 40f;

    [Header("회전")]
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float turnSpeedAtHighSpeed = 70f;

    Rigidbody rb;

    float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 같은 오브젝트에서 자동으로 찾기
        if (input == null)
            input = GetComponent<InputHandler>();

        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.linearDamping = 0f;
        rb.angularDamping = 3f;
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        // 브레이크
        if (input.BrakeInput)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                brakePower * Time.fixedDeltaTime);
        }
        else
        {
            // 전진
            if (input.MoveInput > 0)
            {
                currentSpeed += acceleration * Time.fixedDeltaTime;
            }
            // 후진
            else if (input.MoveInput < 0)
            {
                currentSpeed -= acceleration * Time.fixedDeltaTime;
            }
            // 입력 없음
            else
            {
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    0f,
                    brakePower * 0.3f * Time.fixedDeltaTime);
            }
        }

        currentSpeed = Mathf.Clamp(
            currentSpeed,
            -maxReverseSpeed,
            maxForwardSpeed);

        Vector3 velocity = transform.forward * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    void Turn()
    {
        if (Mathf.Abs(currentSpeed) < 0.1f)
            return;

        float t = Mathf.InverseLerp(
            0,
            maxForwardSpeed,
            Mathf.Abs(currentSpeed));

        float steer = Mathf.Lerp(
            turnSpeed,
            turnSpeedAtHighSpeed,
            t);

        // 후진 시 조향 반전
        float direction = currentSpeed >= 0 ? 1f : -1f;

        rb.MoveRotation(
            rb.rotation *
            Quaternion.Euler(
                0,
                input.SteerInput * steer * direction * Time.fixedDeltaTime,
                0));
    }

    void OnCollisionEnter(Collision collision)
    {
        // 벽에 부딪히면 속도 감소
        currentSpeed *= 0.4f;
    }
}