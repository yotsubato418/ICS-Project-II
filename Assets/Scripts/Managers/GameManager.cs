using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startScreenPanel;
    public GameObject gameOverScreenPanel;

    [Header("Text Displays")]
    public TextMeshProUGUI countdownText; 
    public TextMeshProUGUI scoreText;      
    public TextMeshProUGUI highScoreText;  

    [Header("Player Reference")]
    public MonoBehaviour birdMovementScript; 

    private bool gameHasStarted = false;
    private Rigidbody2D birdRb;
    private float originalGravityScale = 1f;
    private int currentScore = 0;

    private static bool skipStartScreen = false;

    void Start()
    {
        // Freeze time while on the start screen
        Time.timeScale = 0f;

        if (startScreenPanel != null) startScreenPanel.SetActive(true);
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(false);
        
        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "PRESS SPACE TO START";
        }

        // Setup bird physics and disable movement until game starts
        if (birdMovementScript != null)
        {
            birdRb = birdMovementScript.GetComponent<Rigidbody2D>();
            birdMovementScript.enabled = false;

            if (birdRb != null)
            {
                // Save original gravity before disabling physics
                originalGravityScale = birdRb.gravityScale;
                birdRb.gravityScale = 0f;
                birdRb.linearVelocity = Vector2.zero;
            }
        }

        if (skipStartScreen)
        {
            skipStartScreen = false;
            StartGame();
        }
    }

    void Update()
    {
        // Start game on Space key press
        if (!gameHasStarted && startScreenPanel != null && startScreenPanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartGame();
            }
        }

        // Restart game on R key press at Game Over
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
        gameHasStarted = true;

        if (startScreenPanel != null) startScreenPanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

        // Restore bird physics and input controls
        if (birdRb != null)
        {
            birdRb.gravityScale = originalGravityScale;
        }

        if (birdMovementScript != null)
        {
            birdMovementScript.enabled = true;
        }
    }

    public void AddScore(int amount = 1)
    {
        currentScore += amount;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        // Load existing high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update high score if current score is higher
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Assign UI values
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (highScoreText != null) highScoreText.text = "hiscore: " + highScore;

        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        GameOver();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        skipStartScreen = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}