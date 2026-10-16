using UnityEngine;

public class camera_movement : MonoBehaviour
{
    private Transform bird;
    private BirdDeath birdDeath;
    private float xOffset;
    private Vector3 tempPos;

    void Start()
    {
        GameObject birdObj = GameObject.FindWithTag("Player");

        bird = birdObj.transform;
        birdDeath = birdObj.GetComponent<BirdDeath>();

        xOffset = transform.position.x - bird.position.x;
    }

    void FixedUpdate()
    {
        // exit method when bird dies
        if (birdDeath == null || birdDeath.IsDead)
        {
            return;
        }

        tempPos = transform.position;
        tempPos.x = bird.position.x + xOffset;
        transform.position = tempPos;
    }
}