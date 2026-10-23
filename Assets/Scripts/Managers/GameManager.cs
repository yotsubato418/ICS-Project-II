using UnityEngine; //monobehaviour, scenes, buttons, mouse/click, ienumerator, texts
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro; 

public class GameManager : MonoBehaviour //unity gamemaster blueprint
{
    public GameObject startScreenPanel; //public slots
    public GameObject gameOverScreenPanel;
    public TextMeshProUGUI tutorialText; 
    public Movement birdMovementScript; //took variable from movement script 

    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI finalHighScoreText;

    //privates
    private static bool skipTitleOnReload = false; //persists after reload static
    private int currentScore = 0;
    private int highScore = 0;

    void Start() 
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); //check

        if (skipTitleOnReload == true)
        {
            skipTitleOnReload = false; //default
            startScreenPanel.SetActive(false);
            gameOverScreenPanel.SetActive(false);
            StartCoroutine(ActivateGameplayFlow()); //tutorial starts
        }
        else
        {
            Time.timeScale = 0; //pause time for title screen
            startScreenPanel.SetActive(true);
            gameOverScreenPanel.SetActive(false);
            tutorialText.gameObject.SetActive(false);
            birdMovementScript.enabled = false;
        }
    }

    private void OnApplicationQuit() //delete high score when you leave application
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        startScreenPanel.SetActive(false);
        Time.timeScale = 1; //resume time for tut and game
        
        // STOP DELETING THIS CODE ok music starts here joseph liu insert
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

        StartCoroutine(ActivateGameplayFlow()); // tutorial starts 
    }

    IEnumerator ActivateGameplayFlow() 
    {
        birdMovementScript.enabled = false;

        while (Mouse.current.leftButton.isPressed)
        {
            yield return null; //stay here till button is done being pressed
        }

        yield return null; //stay 4 1 more frame

        birdMovementScript.enabled = true; //enables the bird to move (its here cause the loop below freezes it anyways)
        tutorialText.gameObject.SetActive(true);
        tutorialText.text = "PRESS SPACE / CLICK TO START";

        bool pressed = false;
        while (!pressed)
        {
            bool space = (Keyboard.current.spaceKey.wasPressedThisFrame); //sets 2 true
            bool mouse = (Mouse.current.leftButton.wasPressedThisFrame);

            if (space || mouse) pressed = true; //when this is set to true 1 more frame then ends loop
            yield return null;
        }

        tutorialText.gameObject.SetActive(false); 
    }

    public void GameOverWithScore(int inGameScore) //bird death script sends ingamescore as score
    {
        currentScore = inGameScore; //current score, ingamescore, and bird death score match up

        if (currentScore > highScore)
        {
            highScore = currentScore; //sets high score to current score if its applicable
            PlayerPrefs.SetInt("HighScore", highScore); //saves it from reloading scene
            PlayerPrefs.Save();
        }

        Time.timeScale = 0;
        gameOverScreenPanel.SetActive(true);
        finalScoreText.text = currentScore.ToString();
        finalHighScoreText.text = highScore.ToString(); //set text to text
    } //these are in gameover panel so dont needa activate them

    public void OnRetryButtonClicked()
    {
        skipTitleOnReload = true; //for the start method
        Time.timeScale = 1; //lets game flow
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //reload
    }

    public void OnMainMenuButtonClicked()
    {
        skipTitleOnReload = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}