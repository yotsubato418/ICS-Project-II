using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour
{
    [Header("Speed Up Settings")]
    public float multiplier = 2f;
    public float duration = 3f;

    private Movement movement;
    private SpriteRenderer birdSprite;
    private Color originalColor;
    private bool isActive = false;

    void Awake()
    {
        movement = GetComponent<Movement>();
        birdSprite = GetComponent<SpriteRenderer>();
        originalColor = birdSprite.color;
    }

    public void Activate()
    {
        if (!isActive && movement != null)
        {
            StartCoroutine(BoostTimer());
        }
    }

    private IEnumerator BoostTimer()
    {
        isActive = true;

        float originalSpeed =   movement.moveSpeed;
        movement.moveSpeed *= multiplier;

        birdSprite.color = Color.pink;

        yield return new WaitForSeconds(duration);

        movement.moveSpeed = originalSpeed;
        birdSprite.color = originalColor;

        isActive = false;
    }
}
