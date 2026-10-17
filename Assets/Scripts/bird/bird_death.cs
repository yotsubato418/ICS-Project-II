using UnityEngine;

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
        if (!IsDead && collision.gameObject.CompareTag("Pipe"))
        {
            Die();
        }
    }

    void Die()
    {
        IsDead = true;

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
        if (!IsDead || mainCamera == null || birdRenderer == null)
        {
            return;
        }

        float bottomEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).y;

        if (birdRenderer.bounds.max.y < bottomEdge)
        {
            Destroy(gameObject);
        }
    }
}