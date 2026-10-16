using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [Header("Movement")]
    public float flapStrength = 5f;

    [Header("Rotation")]
    public float upwardRotation = 30f;
    public float downwardRotation = -90f;
    public float rotationSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Flap();
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Flap();
        }

        // Rotate based on vertical velocity
        float targetRotation;

        if (rb.linearVelocity.y > 0)
        {
            // Bird moving upward
            targetRotation = upwardRotation;
        }
        else
        {
            // Bird falling
            targetRotation = downwardRotation;
        }

        // Smooth rotation
        float currentRotation = transform.eulerAngles.z;

        float newRotation = Mathf.LerpAngle(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(
            0, 0, newRotation
        );
    }

    void Flap()
    {
        // Reset downward momentum and jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapStrength);

        // Immediately tilt upward
        transform.rotation = Quaternion.Euler(
            0, 0, upwardRotation
        );
    }
}