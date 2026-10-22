using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI & References")]
    public GameObject startScreenPanel;
    public GameObject gameOverScreenPanel;
    public TextMeshProUGUI countdownText; 
    public Movement birdMovementScript;

    [Header("Score Display")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalHighScoreText;

    private static bool skipTitleOnReload = false;
    private int currentScore = 0;
    private int highScore = 0;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);

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
            if (birdMovementScript != null) birdMovementScript.enabled = false;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        if (startScreenPanel != null) startScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ActivateGameplayFlow());
    }

    IEnumerator ActivateGameplayFlow()
    {
        if (birdMovementScript != null) birdMovementScript.enabled = false;

        while (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            yield return null;
        }

        yield return null;

        if (birdMovementScript != null) birdMovementScript.enabled = true;

        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "PRESS SPACE / CLICK TO START";
        }

        bool pressed = false;
        while (!pressed)
        {
            bool space = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);
            bool mouse = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

            if (space || mouse) pressed = true;
            yield return null;
        }

        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    public void GameOverWithScore(int inGameScore)
    {
        currentScore = inGameScore;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        Time.timeScale = 0f;
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = currentScore.ToString();
        if (finalHighScoreText != null) finalHighScoreText.text = highScore.ToString();
    }

    public void OnRetryButtonClicked()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
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