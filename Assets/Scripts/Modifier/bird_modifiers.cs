using UnityEngine;
using System.Collections;

public class BirdModifiers : MonoBehaviour
{
    [Header("Speed Modifiers")]
    [SerializeField] private float speedUpMultiplier = 2f;
    [SerializeField] private float slowDownMultiplier = 0.5f;
    [SerializeField] private float speedDuration = 3f;

    [Header("Gravity and Direction")]
    [SerializeField] private float gravityDuration = 4f;
    [SerializeField] private float directionDuration = 3f;

    [Header("Size Modifier")]
    [SerializeField] private float shrinkMultiplier = 0.6f;
    [SerializeField] private float shrinkDuration = 5f;

    [Header("Ghost Modifier")]
    [SerializeField] private float ghostDuration = 4f;
    [SerializeField] private string pipeLayerName = "Pipes";
    [SerializeField, Range(0f, 1f)]
    private float ghostOpacity = 0.6f;

    private Movement movement;
    private SpriteRenderer birdSprite;
    private Vector3 originalScale;
    private Color originalColor;
    private bool originalFlipY;

    private int birdLayer;
    private int pipeLayer;

    private bool speedUpActive;
    private bool slowDownActive;
    private bool gravityActive;
    private bool directionActive;
    private bool shrinkActive;
    private bool ghostActive;
    private bool cheatGhostActive = false;

    void Awake()
    {
        movement = GetComponent<Movement>();
        birdSprite = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;

        if (birdSprite != null)
        {
            originalColor = birdSprite.color;
            originalFlipY = birdSprite.flipY;
        }

        birdLayer = gameObject.layer;
        pipeLayer = LayerMask.NameToLayer(pipeLayerName);

        if (pipeLayer == -1)
            Debug.LogWarning("Pipes layer has not been created!");
    }

    public void ActivateModifier(Modifier.ModifierType type)
    {
        switch (type)
        {
            case Modifier.ModifierType.SpeedUp:
                if (!speedUpActive)
                    StartCoroutine(SpeedUpEffect());
                break;

            case Modifier.ModifierType.SlowDown:
                if (!slowDownActive)
                    StartCoroutine(SlowDownEffect());
                break;

            case Modifier.ModifierType.FlipGravity:
                if (!gravityActive)
                    StartCoroutine(FlipGravityEffect());
                break;

            case Modifier.ModifierType.FlipDirection:
                if (!directionActive)
                    StartCoroutine(FlipDirectionEffect());
                break;

            case Modifier.ModifierType.MakesSmaller:
                if (!shrinkActive)
                    StartCoroutine(ShrinkEffect());
                break;

            case Modifier.ModifierType.Ghost:
                if (!ghostActive && pipeLayer >= 0)
                    StartCoroutine(GhostEffect());
                break;
        }
    }

    // recalculate speed when effects overlap
    private void UpdateSpeed()
    {
        float multiplier = 1f;

        if (speedUpActive)
            multiplier *= speedUpMultiplier;

        if (slowDownActive)
            multiplier *= slowDownMultiplier;

        movement.speedMultiplier = multiplier;
    }

    // choose colour based on active modifiers
    private void UpdateColor()
    {
        if (birdSprite == null)
            return;

        Color color = originalColor;

        if (gravityActive)
            color = Color.yellow;
        else if (directionActive)
            color = Color.cyan;
        else if (shrinkActive)
            color = Color.green;
        else if (slowDownActive)
            color = Color.blue;
        else if (speedUpActive)
            color = Color.pink;

        if (ghostActive || cheatGhostActive)
            color.a = originalColor.a * ghostOpacity;
        else
            color.a = originalColor.a;

        birdSprite.color = color;
    }

    private IEnumerator SpeedUpEffect()
    {
        speedUpActive = true;
        UpdateSpeed();
        UpdateColor();

        yield return new WaitForSeconds(speedDuration);

        speedUpActive = false;
        UpdateSpeed();
        UpdateColor();
    }

    private IEnumerator SlowDownEffect()
    {
        slowDownActive = true;
        UpdateSpeed();
        UpdateColor();

        yield return new WaitForSeconds(speedDuration);

        slowDownActive = false;
        UpdateSpeed();
        UpdateColor();
    }

    private IEnumerator FlipGravityEffect()
    {
        gravityActive = true;
        movement.gravityFlipped = true;

        if (birdSprite != null)
            birdSprite.flipY = !originalFlipY;

        UpdateColor();

        yield return new WaitForSeconds(gravityDuration);

        movement.gravityFlipped = false;
        gravityActive = false;

        if (birdSprite != null)
            birdSprite.flipY = originalFlipY;

        UpdateColor();
    }

    private IEnumerator FlipDirectionEffect()
    {
        directionActive = true;
        movement.direction = -1;
        UpdateColor();

        yield return new WaitForSeconds(directionDuration);

        movement.direction = 1;
        directionActive = false;
        UpdateColor();
    }

    private IEnumerator ShrinkEffect()
    {
        shrinkActive = true;

        transform.localScale =
            originalScale * shrinkMultiplier;

        UpdateColor();

        yield return new WaitForSeconds(shrinkDuration);

        transform.localScale = originalScale;
        shrinkActive = false;
        UpdateColor();
    }

    private IEnumerator GhostEffect()
    {
        ghostActive = true;
        UpdateGhostState();

        yield return new WaitForSeconds(ghostDuration);

        ghostActive = false;
        UpdateGhostState();
    }

    private void OnDisable()
    {
        if ((ghostActive || cheatGhostActive) && pipeLayer >= 0)
        {
            Physics2D.IgnoreLayerCollision(
                birdLayer, pipeLayer, false
            );
        }

        if (movement != null)
        {
            movement.speedMultiplier = 1f;
            movement.gravityFlipped = false;
            movement.direction = 1;
        }

        speedUpActive = false;
        slowDownActive = false;
        gravityActive = false;
        directionActive = false;
        shrinkActive = false;
        ghostActive = false;
        cheatGhostActive = false;

        transform.localScale = originalScale;

        if (birdSprite != null)
        {
            birdSprite.flipY = originalFlipY;
            birdSprite.color = originalColor;
        }
    }


    public void ActivateGhostCheat()
    {
        if (cheatGhostActive)
            return;

        cheatGhostActive = true;
        UpdateGhostState();
    }

    public void DeactivateGhostCheat()
    {
        if (!cheatGhostActive)
            return;

        cheatGhostActive = false;
        UpdateGhostState();
    }

    private void UpdateGhostState()
    {
        // ghost stays enabled if either the normal
        // modifier or the cheat is active.
        bool shouldBeGhost = ghostActive || cheatGhostActive;

        if (pipeLayer >= 0)
        {
            Physics2D.IgnoreLayerCollision(
                birdLayer,
                pipeLayer,
                shouldBeGhost
            );
        }

        UpdateColor();
    }

}