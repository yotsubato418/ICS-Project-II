using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdDeath : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;
    public float deathGravityScale = 1f;

    private Rigidbody2D rb;
    private Collider2D birdCollider;
    private SpriteRenderer birdRenderer;
    private Camera mainCamera;
    private Score score;
    private Movement movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<Collider2D>();
        birdRenderer = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;
        score = GetComponent<Score>();
        movement = GetComponent<Movement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead) return;

        if (collision.gameObject.CompareTag("upperPipe"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayTopHit();
            }
            Die();
        }
        else if (collision.gameObject.CompareTag("lowerPipe"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayBottomHit();
            }
            Die();
        }
        else if (collision.gameObject.CompareTag("Upper Limit"))
        {
            Die();
        }
    }

    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        // 1. Grab the final score safely before resetting
        int finalScore = 0;
        if (score != null)
        {
            // Accessing the score variable from your Score.cs script
            finalScore = score.score; 
        }

        // 2. Send the score to the GameManager
        FindFirstObjectByType<GameManager>().GameOverWithScore(finalScore);

        if (score != null)
        {
            score.ResetScore();
        }

        // Stop horizontal movement and bounce upward
        rb.gravityScale = deathGravityScale;
        if (movement != null)
        {
            rb.linearVelocity = new Vector2(0, movement.flapStrength);
        }
        
        // Prevent additional collisions
        birdCollider.enabled = false;

        // Immediately rotate upward
        if (movement != null)
        {
            transform.rotation = Quaternion.Euler(0, 0, movement.upwardRotation);
        }
    }

    void Update()
    {
        if (mainCamera == null || birdRenderer == null)
        {
            return;
        }

        float bottomEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        if (birdRenderer.bounds.max.y < bottomEdge)
        {
            Die();
        }
    }
}