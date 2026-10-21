using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels & Text")]
    public GameObject startScreenPanel;
    public GameObject gameOverScreenPanel;
    public TextMeshProUGUI countdownText; 
    public Movement birdMovementScript; // Your teammate's Movement script

    [Header("Score UI")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI finalScoreText;     // Shown on Game Over screen
    public TextMeshProUGUI finalHighScoreText; // Shown on Game Over screen

    private static bool skipTitleOnReload = false;
    private int currentScore = 0;
    private int highScore = 0;

    void Start()
    {
        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();

        if (skipTitleOnReload)
        {
            skipTitleOnReload = false;

            if (startScreenPanel != null) startScreenPanel.SetActive(false);
            if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(false);
            
            StartCoroutine(ActivateGameplayFlow());
        }
        else
        {
            Time.timeScale = 0f;
            if (startScreenPanel != null) startScreenPanel.SetActive(true);
            if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(false);
            if (countdownText != null) countdownText.gameObject.SetActive(false);

            // Keep movement disabled on the title screen so it ignores menu clicks
            if (birdMovementScript != null)
            {
                birdMovementScript.enabled = false;
            }
        }
    }

    public void StartGame()
    {
        if (startScreenPanel != null) startScreenPanel.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(ActivateGameplayFlow());
    }

    IEnumerator ActivateGameplayFlow()
    {
        // 1. Keep bird movement disabled while waiting for the menu button click to release
        if (birdMovementScript != null)
        {
            birdMovementScript.enabled = false;
        }

        while (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            yield return null;
        }

        yield return null; // Extra frame buffer

        // 2. Enable bird movement so its Start() runs and locks gravity to 0
        if (birdMovementScript != null)
        {
            birdMovementScript.enabled = true;
        }

        // 3. Show the caption text cleanly
        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "PRESS SPACE / CLICK TO START";
        }

        // 4. Wait for a fresh click or space press to hide the caption
        bool pressed = false;
        while (!pressed)
        {
            bool space = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);
            bool mouse = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

            if (space || mouse)
            {
                pressed = true;
            }
            yield return null;
        }

        // 5. Hide the caption text the exact moment you click
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    // --- SCORE METHODS ---

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (currentScoreText != null) currentScoreText.text = "Score: " + currentScore;
        if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
    }

    // --- GAME OVER & RESTART ---

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(true);

        // Populate Game Over text fields
        if (finalScoreText != null) finalScoreText.text = "Score: " + currentScore;
        if (finalHighScoreText != null) finalHighScoreText.text = "High Score: " + highScore;
    }

    public void TriggerGameOver()
    {
        GameOver();
    }

    public void OnRetryButtonClicked()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        skipTitleOnReload = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuButtonClicked()
    {
        skipTitleOnReload = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}