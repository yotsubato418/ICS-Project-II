using UnityEngine;
using System.Collections.Generic;

public class background_rep : MonoBehaviour
{
    public GameObject bgPrefab;
    public int maxBlock = 2;
    public int minBlock = 1;

    private float width;
    private float nextX;
    private Queue<GameObject> blocks = new Queue<GameObject>();
    void Start()
    {
        width = bgPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * bgPrefab.transform.localScale.x;
        nextX = transform.position.x - 2 * width;
    }

    // Update is called once per frame
    void Update()
    {
        while (nextX < transform.position.x + maxBlock * width)
        {
            Vector3 pos = new Vector3(
                nextX,
                bgPrefab.transform.position.y,
                bgPrefab.transform.position.z
            );
            blocks.Enqueue(Instantiate(bgPrefab, pos, Quaternion.identity));
            nextX += width;
        }

        while (blocks.Count > 0 && blocks.Peek().transform.position.x < transform.position.x - minBlock * width)
        {
            Destroy(blocks.Dequeue());
        }
    }
}
