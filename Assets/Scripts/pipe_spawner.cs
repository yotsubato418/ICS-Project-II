using UnityEngine;
using System.Collections.Generic;

public class pipe_spawner : MonoBehaviour
{
    public GameObject upperPipe;
    public GameObject lowerPipe;
    public float gapX = 4f;
    public float gapY = 2f;
    public float minY = -1.5f;
    public float maxY = 1.5f;
    public float startOffset = 3f;
    public int maxPipe = 5;
    public int minPipe = 2;

    private float upperBottomOffset;
    private float lowerTopOffset;

    private float nextX;
    private Queue<GameObject> pipes = new Queue<GameObject>();
    void Start()
    {
        nextX = transform.position.x + startOffset;
        upperBottomOffset = GetBottomOffset(upperPipe);
        lowerTopOffset = GetTopOffset(lowerPipe);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        while (nextX < transform.position.x + maxPipe * gapX)
        {
            float centerY = Random.Range(minY, maxY);

            Vector3 upperPos = new Vector3(nextX, centerY + gapY / 2f - upperBottomOffset, upperPipe.transform.position.z);
            Vector3 lowerPos = new Vector3(nextX, centerY - gapY / 2f - lowerTopOffset, lowerPipe.transform.position.z);
            pipes.Enqueue(Instantiate(upperPipe, upperPos, Quaternion.identity));
            pipes.Enqueue(Instantiate(lowerPipe, lowerPos, Quaternion.identity));
            nextX += gapX;
        }

        while (pipes.Count > 0 && pipes.Peek().transform.position.x < transform.position.x - minPipe * gapX)
        {
            Destroy(pipes.Dequeue());
        }
    }

    float GetBottomOffset(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        float scaleY = Mathf.Abs(sr.transform.lossyScale.y);
        float childY = sr.transform.position.y - prefab.transform.position.y;
        return childY + sr.sprite.bounds.min.y * scaleY;
    }

    float GetTopOffset(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        float scaleY = Mathf.Abs(sr.transform.lossyScale.y);
        float childY = sr.transform.position.y - prefab.transform.position.y;
        return childY + sr.sprite.bounds.max.y * scaleY;
    }
}
