using UnityEngine;
using System.Collections.Generic;

public class pipe_spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject upperPipe;
    public GameObject lowerPipe;
    public GameObject scoreZone;

    [Header("Horizontal Spawning")]
    public float gapX = 4f;
    public float gapY = 2f;
    public float startOffset = 3f;
    public int maxPipeRender = 5;
    public int minPipeRender = 5;

    [Header("Vertical Spawning")]
    public float universalMinY = -2f;
    public float universalMaxY = 1.5f;
    public float maxVerticalStep = 3f;

    [Header("Pipe Positioning")]
    public float upperPipeXOffset = 0.5f;

    [System.Serializable]
    public class ModifierPrefab
    {
        public Modifier.ModifierType type;
        public GameObject prefab;
    }

    [Header("Modifier Settings")]
    [Range(0f, 1f)]
    public float modifierChance = 0.5f;

    public int minPipesBetweenModifiers = 10;

    public ModifierPrefab[] modifierPrefabs;

    private int lastModifierPipe = 0;

    private float upperBottomOffset;
    private float lowerTopOffset;

    public int pipeCount = 0;

    private float nextX;
    private float previousY;
    private bool hasPreviousPipe = false;

    private Camera cam;
    private Queue<GameObject> pipes = new Queue<GameObject>();

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("Pipe spawner needs a Camera component!");
            enabled = false;
            return;
        }

        nextX = transform.position.x + startOffset;

        upperBottomOffset = GetBottomOffset(upperPipe);
        lowerTopOffset = GetTopOffset(lowerPipe);
    }

    void LateUpdate()
    {
        while (nextX < transform.position.x + maxPipeRender * gapX)
        {
            float centerY = GetNextPipeY();

            Vector3 upperPos = new Vector3(
                nextX + upperPipeXOffset,
                centerY + gapY / 2f - upperBottomOffset,
                upperPipe.transform.position.z
            );

            Vector3 lowerPos = new Vector3(
                nextX,
                centerY - gapY / 2f - lowerTopOffset,
                lowerPipe.transform.position.z
            );

            Vector3 zonePos = new Vector3(
                nextX,
                centerY,
                upperPipe.transform.position.z
            );

            pipes.Enqueue(Instantiate(
                upperPipe, upperPos, Quaternion.identity
            ));

            pipes.Enqueue(Instantiate(
                lowerPipe, lowerPos, Quaternion.identity
            ));


            if (pipeCount - lastModifierPipe >= minPipesBetweenModifiers
                && Random.value < modifierChance)
            {
                // collect all configured modifier prefabs
                List<ModifierPrefab> availableModifiers =
                    new List<ModifierPrefab>();

                foreach (ModifierPrefab entry in modifierPrefabs)
                {
                    if (entry != null && entry.prefab != null)
                    {
                        availableModifiers.Add(entry);
                    }
                }

                if (availableModifiers.Count > 0)
                {
                    // randomly select a modifier
                    int index = Random.Range(
                        0, availableModifiers.Count
                    );

                    ModifierPrefab selectedModifier =
                        availableModifiers[index];

                    Vector3 modifierPos = new Vector3(
                        nextX,
                        centerY,
                        upperPipe.transform.position.z
                    );

                    GameObject modifier = Modifier.Spawn(
                        selectedModifier.prefab,
                        modifierPos,
                        selectedModifier.type
                    );

                    if (modifier != null)
                    {
                        pipes.Enqueue(modifier);
                        lastModifierPipe = pipeCount;
                    }
                }
            }


            GameObject zone = Instantiate(
                scoreZone, zonePos, Quaternion.identity
            );

            zone.transform.localScale = new Vector3(
                zone.transform.localScale.x,
                gapY,
                1f
            );

            pipes.Enqueue(zone);

            nextX += gapX;
            pipeCount++;
        }

        while (pipes.Count > 0 &&
            pipes.Peek().transform.position.x <
            transform.position.x - minPipeRender * gapX)
        {
            Destroy(pipes.Dequeue());
        }
    }

    // calculate the next pipe's vertical position
    float GetNextPipeY()
    {
        float minSpawnY = universalMinY;
        float maxSpawnY = universalMaxY;

        if (hasPreviousPipe)
        {
            minSpawnY = Mathf.Max(
                universalMinY,
                previousY - maxVerticalStep
            );

            maxSpawnY = Mathf.Min(
                universalMaxY,
                previousY + maxVerticalStep
            );
        }

        float newY = Random.Range(minSpawnY, maxSpawnY);

        previousY = newY;
        hasPreviousPipe = true;

        return newY;
    }

    float GetBottomOffset(GameObject prefab)
    {
        SpriteRenderer sr =
            prefab.GetComponentInChildren<SpriteRenderer>();

        float scaleY = Mathf.Abs(sr.transform.lossyScale.y);
        float childY = sr.transform.position.y
            - prefab.transform.position.y;

        return childY + sr.sprite.bounds.min.y * scaleY;
    }

    float GetTopOffset(GameObject prefab)
    {
        SpriteRenderer sr =
            prefab.GetComponentInChildren<SpriteRenderer>();

        float scaleY = Mathf.Abs(sr.transform.lossyScale.y);
        float childY = sr.transform.position.y
            - prefab.transform.position.y;

        return childY + sr.sprite.bounds.max.y * scaleY;
    }
}