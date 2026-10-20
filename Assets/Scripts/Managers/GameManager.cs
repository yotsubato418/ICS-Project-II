using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject startScreenPanel;
    public GameObject gameOverScreenPanel;
    public TextMeshProUGUI countdownText; 
    public Movement birdMovementScript; // Drag your teammate's Movement script here in the Inspector!

    private static bool skipTitleOnReload = false;

    void Start()
    {
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

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

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverScreenPanel != null) gameOverScreenPanel.SetActive(true);
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