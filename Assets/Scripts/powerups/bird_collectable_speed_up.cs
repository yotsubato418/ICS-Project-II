using UnityEngine;
using System.Collections;

public class BirdSpeedBoost : MonoBehaviour // the first class, basically attaches stuff to gameobject!
{
    private SpeedBoost powerUp;
    private Movement movement;
    private SpriteRenderer birdSprite;
    private Color originalColor;

    void Awake()
    {
        movement = GetComponent<Movement>();
        powerUp = new SpeedBoost(2f, 3f);
        birdSprite = GetComponent<SpriteRenderer>();
        originalColor = birdSprite.color;
    }

    public void ActivateBoost()
    {
        if (movement != null && !powerUp.IsActive)
        {
            powerUp.Activate();
            StartCoroutine(BoostTimer());
        }
    }

    IEnumerator BoostTimer()
    {
        float originalSpeed = movement.moveSpeed;
        movement.moveSpeed *= powerUp.Multiplier;

        birdSprite.color = Color.pink;


        yield return new WaitForSeconds(powerUp.Duration);

        movement.moveSpeed = originalSpeed;
        birdSprite.color = originalColor;
        powerUp.Deactivate();
    }
}

public class SpeedBoost
{
    private float multiplier;
    private float duration;
    private bool isActive;

    public float Multiplier
    {
        get { return multiplier; }
    }

    public float Duration
    {
        get { return duration; }
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    public SpeedBoost(float boostMultiplier, float boostDuration)
    {
        multiplier = boostMultiplier;
        duration = boostDuration;
        isActive = false;
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }
}