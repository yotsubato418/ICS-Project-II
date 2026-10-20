using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdDeath : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;

    public float deathGravityScale = 1f;

    private Rigidbody2D rb;
    private Collider2D birdCollider;
    private SpriteRenderer birdRenderer;
    private Movement movement;
    private Camera mainCamera;
    private Score score;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<Collider2D>();
        birdRenderer = GetComponentInChildren<SpriteRenderer>();
        movement = GetComponent<Movement>();
        mainCamera = Camera.main;
        score = GetComponent<Score>();
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
    }

    void Die()
    {
        if (IsDead) return; 
        IsDead = true; 

        FindFirstObjectByType<GameManager>().TriggerGameOver(); 

        if (score != null)
        {
            score.ResetScore();
        }

        // stop horizontal movement and bounce upward
        rb.gravityScale = deathGravityScale;
        rb.linearVelocity = new Vector2(0, movement.flapStrength);

        // prevent additional collisions
        birdCollider.enabled = false;

        // immediately rotate upward
        transform.rotation = Quaternion.Euler(
            0, 0, movement.upwardRotation
        );
    }

    void Update()
    {
        if (mainCamera == null || birdRenderer == null)
        {
            return;
        }

        float bottomEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).y;

        if (birdRenderer.bounds.max.y < bottomEdge)
        {
            Die(); 
        }
    }
}