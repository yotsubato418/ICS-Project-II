using UnityEngine;
using UnityEngine.InputSystem;

public class KonamiCode : MonoBehaviour
{
    [Header("Cheat Speed Settings")]
    [SerializeField] private float scrollSpeedStep = 0.5f;
    [SerializeField] private float minCheatSpeed = 0f;
    [SerializeField] private float maxCheatSpeed = 15f;

    private Movement movement;
    private float originalSpeed;
    private BirdModifiers birdModifiers;

    private Key[] cheatCode =
    {
        Key.UpArrow,
        Key.UpArrow,
        Key.DownArrow,
        Key.DownArrow,
        Key.LeftArrow,
        Key.RightArrow,
        Key.LeftArrow,
        Key.RightArrow,
        Key.B,
        Key.A,
        Key.Enter
    };

    private int currentKey = 0;
    private bool cheatActive = false;

    void Awake()
    {
        birdModifiers = GetComponent<BirdModifiers>();
        movement = GetComponent<Movement>();

        if (movement != null)
            originalSpeed = movement.moveSpeed;
    }

    void Update()
    {
        if (Keyboard.current == null || birdModifiers == null)
            return;

        // esc disables permanent ghost mode
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (cheatActive)
            {
                birdModifiers.DeactivateGhostCheat();

                if (movement != null)
                    movement.moveSpeed = originalSpeed;

                cheatActive = false;
            }

            currentKey = 0;
            return;
        }

        // don't process a new code while activated
        if (cheatActive)
        {
            HandleScrollSpeed();
            return;
        }

        foreach (KeyControlWrapper key in GetPressedKeys())
        {
            CheckKey(key.key);
        }
    }

    private void CheckKey(Key pressedKey)
    {
        if (pressedKey == cheatCode[currentKey])
        {
            currentKey++;

            if (currentKey >= cheatCode.Length)
            {
                birdModifiers.ActivateGhostCheat();
                cheatActive = true;
                currentKey = 0;
            }
        }
        else
        {
            // incorrect key resets the sequence
            // an up arrow can also start a new sequence
            currentKey = pressedKey == cheatCode[0] ? 1 : 0;
        }
    }

    private struct KeyControlWrapper
    {
        public Key key;
    }

    private System.Collections.Generic.IEnumerable<KeyControlWrapper>
        GetPressedKeys()
    {
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                yield return new KeyControlWrapper
                {
                    key = key.keyCode
                };
            }
        }
    }


    private void HandleScrollSpeed()
    {
        if (!cheatActive || movement == null || Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll == 0f)
            return;

        // scroll up increases speed, down decreases it
        float direction = Mathf.Sign(scroll);

        movement.moveSpeed += direction * scrollSpeedStep;

        // keep speed within the allowed range
        movement.moveSpeed = Mathf.Clamp(
            movement.moveSpeed,
            minCheatSpeed,
            maxCheatSpeed
        );
    }
}