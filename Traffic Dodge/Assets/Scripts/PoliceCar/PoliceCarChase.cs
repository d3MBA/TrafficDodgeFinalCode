using UnityEngine;

public class PoliceCarChase : MonoBehaviour
{
    [SerializeField]
    private Transform playerCar;

    [SerializeField]
    private float chaseSpeed = 20f;

    [SerializeField]
    private float turnSpeed = 5f;

    [SerializeField]
    private float stoppingDistance = 5f; // Distance to maintain when near the player car

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure the player car is assigned
        if (playerCar == null)
        {
            // Find the player car by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerCar = playerObj.transform;
            }
            else
            {
                Debug.LogError("player car not found.'.");
            }
        }
    }

    void FixedUpdate()
    {
        if (playerCar == null) return;

        // Calculate direction towards the player car
        Vector3 direction = (playerCar.position - transform.position);
        float distance = direction.magnitude;

        // Stop moving if within stopping distance
        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        direction.Normalize();

        // Move the police car forward
        Vector3 move = direction * chaseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate smoothly towards the player car
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rotation);
    }
}
