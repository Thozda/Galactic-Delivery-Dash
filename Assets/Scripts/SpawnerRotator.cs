using UnityEngine;

public class SpawnerRotator : MonoBehaviour
{
    private Vector2 previousPosition; // Tracks the previous position of PlayerPos
    public GameObject PlayerPos; // Reference to the player GameObject
    public float rotationSpeed = 5f; // Speed of rotation

    void Start()
    {
        // Ensure PlayerPos is assigned and initialize previous position
        if (PlayerPos != null)
        {
            previousPosition = PlayerPos.transform.position;
        }
        else
        {
            Debug.LogError("PlayerPos is not assigned. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (PlayerPos == null) return;

        // Get the current position of PlayerPos
        Vector2 currentPosition = PlayerPos.transform.position;

        // Calculate movement direction
        Vector2 movementDirection = currentPosition - previousPosition;

        // If the player moved, rotate the object
        if (movementDirection.sqrMagnitude > 0.001f) // Avoid rotating for negligible movement
        {
            // Calculate the angle in degrees
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

            // Smoothly rotate the spawner to face the direction
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle + 90, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
        }

        // Update the previous position
        previousPosition = currentPosition;
    }
}
