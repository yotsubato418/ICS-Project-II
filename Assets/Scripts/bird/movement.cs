using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float flapStrength = 5f;
    [HideInInspector] public float speedMultiplier = 1f;
    [HideInInspector] public int direction = 1;
    [HideInInspector] public bool gravityFlipped = false;

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

        if (!started)
        {
            return;
        }


        float targetRotation;

        float gravityDirection = gravityFlipped ? -1f : 1f;

        if (rb.linearVelocity.y * gravityDirection > 0)
        {
            targetRotation = upwardRotation;
        }
        else
        {
            targetRotation = downwardRotation;
        }

        if (gravityFlipped)
        {
            targetRotation *= -1f;
        }

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

    void FixedUpdate()
    {
        if (!started || birdDeath.IsDead)
            return;

        rb.gravityScale = originalGravity *
            (gravityFlipped ? -1f : 1f);

        rb.linearVelocity = new Vector2(
            moveSpeed * speedMultiplier * direction,
            rb.linearVelocity.y
        );
    }

    void Flap()
    {
        float flapDirection = gravityFlipped ? -1f : 1f;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            flapStrength * flapDirection
        );

        transform.rotation = Quaternion.Euler(
            0, 0,
            upwardRotation * flapDirection
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