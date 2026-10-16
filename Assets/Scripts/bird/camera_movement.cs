using UnityEngine;

public class camera_movement : MonoBehaviour
{
    private Transform camera;
    private float xOffset;
    private Vector3 tempPos;
    void Start()
    {
        GameObject birdObj = GameObject.FindWithTag("Player");
        camera = birdObj.transform;
        xOffset = transform.position.x - camera.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempPos = transform.position;
        tempPos.x = camera.position.x + xOffset;
        transform.position = tempPos;
    }
}
