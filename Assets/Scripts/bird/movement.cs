using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float flapStrength = 5f;

    [Header("Rotation")]
    public float upwardRotation = 30f;
    public float downwardRotation = -90f;
    public float rotationSpeed = 5f;

    private Rigidbody2D rb;
    private BirdDeath birdDeath;
    private bool started = false;
    private float originalGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        birdDeath = GetComponent<BirdDeath>();

        // save gravity and disable it until the game starts
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // keep bird still before starting
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (!birdDeath.IsDead)
        {
            bool pressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
            
            if (pressed)
            {
                if (!started)
                {
                    started = true;
                    rb.gravityScale = originalGravity;
                }

                Flap();
            }
        }

        // if not started exit method
        if (!started)
        {
            return;
        }

        // rotate based on vertical velocity
        float targetRotation;

        if (rb.linearVelocity.y > 0)
        {
            targetRotation = upwardRotation; // precaution
        }
        else
        {
            targetRotation = downwardRotation;
        }

        // rotation lerp
        float currentRotation = transform.eulerAngles.z;

        float newRotation = Mathf.LerpAngle(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    void FixedUpdate()
    {
        if (!started || birdDeath.IsDead)
        {
            return;
        }

        // constant horizontal movement
        rb.linearVelocity = new Vector2(
            moveSpeed,
            rb.linearVelocity.y
        );
    }

    void Flap()
    {
        // reset vertical velocity and jump
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            flapStrength
        );

        // immediately rotate upward
        transform.rotation = Quaternion.Euler(
            0, 0, upwardRotation
        );
        PlayFlapSound();
    }

    public void PlayFlapSound()
    {
        if (!started || birdDeath.IsDead)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFlap();
        }
    }
}