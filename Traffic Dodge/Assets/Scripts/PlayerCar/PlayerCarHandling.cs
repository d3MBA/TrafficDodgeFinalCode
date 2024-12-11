using UnityEngine;

public class PlayerCarHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform gameModel;

    // max values
    private float maxSteerVelocity = 2f;
    private float maxForwardVelocity = 30f;

    // multipliers
    private float accelerationMultiplier = 3f;
    private float steeringMultiplier = 5f;

    // input
    private Vector2 input = Vector2.zero;

    void Start()
    {
        // make sure rigidbody is assigned
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // make sure game model is assigned
        if (gameModel == null)
            gameModel = transform;
    }

    void Update()
    {
        // rotate car model when turning
        gameModel.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5f, 0);
    }

    void FixedUpdate()
    {
        // apply acceleration
        Accelerate();

        // steer the car
        Steer();

        // prevent the car from going backwards
        if (rb.linearVelocity.z < 0)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
    }

    void Accelerate()
    {
        // limit the speed
        if (rb.linearVelocity.z < maxForwardVelocity)
        {
            // move the car forward
            Vector3 force = transform.forward * accelerationMultiplier;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            // move the car sideways
            float speedBasedSteerLimit = rb.linearVelocity.z / 5f;
            speedBasedSteerLimit = Mathf.Clamp01(speedBasedSteerLimit);

            Vector3 steerForce = transform.right * steeringMultiplier * input.x * speedBasedSteerLimit;
            rb.AddForce(steerForce, ForceMode.Acceleration);

            // limit sideways speed
            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);

            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else
        {
            // auto-center car
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z), Time.fixedDeltaTime * 3f);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        input = inputVector.normalized;
    }
    
}
