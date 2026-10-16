using UnityEngine;
using UnityEngine.InputSystem;

public class move_right : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D _rigidBody;
    private bool started = false;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            started = true;
        }
    }
    void FixedUpdate()
    {
        if (!started){
            return;
        }
        _rigidBody.linearVelocity = new Vector2(moveSpeed, _rigidBody.linearVelocity.y);
    }
}
