using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject startScreenPanel;
    public GameObject gameOverScreenPanel;
    public TextMeshProUGUI countdownText; // Drag your CountdownText here in Inspector
    public MonoBehaviour birdMovementScript; // Drag your Bird's movement script here

    void Start()
    {
        // Freeze time and show start screen when game launches
        Time.timeScale = 0f;
        if (startScreenPanel != null) startScreenPanel.SetActive(true);
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check for R key to restart when Game Over screen is active
        if (gameOverScreenPanel != null && gameOverScreenPanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartGame();
            }
        }
    }

    public void StartGame()
    {
        if (startScreenPanel != null) startScreenPanel.SetActive(false); // Hide title screen
        StartCoroutine(StartCountdownRoutine());
    }

    IEnumerator StartCountdownRoutine()
    {
        // Get the Bird's Rigidbody2D component
        Rigidbody2D rb = null;
        if (birdMovementScript != null)
        {
            rb = birdMovementScript.GetComponent<Rigidbody2D>();
            birdMovementScript.enabled = false; // Disable player input
        }

        // Freeze physics/gravity during countdown
        float originalGravity = 1f;
        if (rb != null)
        {
            originalGravity = rb.gravityScale; // Save original gravity setting
            rb.gravityScale = 0f;              // Turn off gravity so pigeon stays frozen
            rb.linearVelocity = Vector2.zero;  // Stop any existing movement
        }

        Time.timeScale = 1f; // Unfreeze time so coroutine runs smoothly

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            countdownText.text = "4\nPRESS SPACE/CLICK TO JUMP - nathan";
            yield return new WaitForSeconds(1f);

            countdownText.text = "3\nPRESS SPACE/CLICK TO JUMP - nathan";
            yield return new WaitForSeconds(1f);

            countdownText.text = "2\nPRESS SPACE/CLICK TO JUMP - nathan";
            yield return new WaitForSeconds(1f);

            countdownText.text = "1\nPRESS SPACE/CLICK TO JUMP - nathan";
            yield return new WaitForSeconds(1f);

            countdownText.text = "bye\nbye";
            yield return new WaitForSeconds(0.5f);

            countdownText.gameObject.SetActive(false); // Hide text when done
        }

        // Restore gravity & re-enable bird controls!
        if (rb != null)
        {
            rb.gravityScale = originalGravity;
        }

        if (birdMovementScript != null)
        {
            birdMovementScript.enabled = true;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        GameOver();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 