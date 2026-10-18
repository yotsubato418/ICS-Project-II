    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class pipe_spawner : MonoBehaviour
    {
        public GameObject upperPipe;
        public GameObject lowerPipe;
        public GameObject scoreZone;
        public GameObject shieldPrefab;//shield prefab
        
        public float gapX = 4f;
        public float gapY = 2f;
        public float minY = -1.5f;
        public float maxY = 1.5f;
        public float startOffset = 3f;
        public int maxPipe = 5;
        public int minPipe = 5;

        private float upperBottomOffset;
        private float lowerTopOffset;
        public int pipeCount = 0; //counts # of pipes for shield spawning

        private float nextX;
        private Queue<GameObject> pipes = new Queue<GameObject>();
        void Start()
        {
            nextX = transform.position.x + startOffset;
            upperBottomOffset = GetBottomOffset(upperPipe);
            lowerTopOffset = GetTopOffset(lowerPipe);
        }

        void LateUpdate()
        {
            while (nextX < transform.position.x + maxPipe * gapX)
            {
                float centerY = UnityEngine.Random.Range(minY, maxY);

                Vector3 upperPos = new Vector3(nextX, centerY + gapY / 2f - upperBottomOffset, upperPipe.transform.position.z);
                Vector3 lowerPos = new Vector3(nextX, centerY - gapY / 2f - lowerTopOffset, lowerPipe.transform.position.z);
                Vector3 zonePos = new Vector3(nextX, centerY, upperPipe.transform.position.z);

                pipes.Enqueue(Instantiate(upperPipe, upperPos, Quaternion.identity));
                pipes.Enqueue(Instantiate(lowerPipe, lowerPos, Quaternion.identity));


                if (pipeCount % 10 == 0) //collectable, every 10 pipes there's a collectable
                {
                     float shieldY = centerY; //positions shield in the middle of pipes

                    Vector3 shieldPos = new Vector3(
                        nextX,
                        shieldY,
                        upperPipe.transform.position.z
                    );
                    

                    GameObject shield = Instantiate(shieldPrefab, shieldPos, Quaternion.identity); //creates new shield using prefab
                    shield.SetActive(true);
                    shield.name = "TEST SHIELD";
                    Debug.Log("Shield spawned at: " + shield.transform.position);//more testing stuff

                    pipes.Enqueue(shield);
                }
                

                GameObject zone = Instantiate(scoreZone, zonePos, Quaternion.identity);
                zone.transform.localScale = new Vector3(zone.transform.localScale.x, gapY, 1f);
                pipes.Enqueue(zone);

                nextX += gapX;
                pipeCount++; //adds pipes by 1 each time!

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
