using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.fontSize = 60;
        scoreText.color = Color.red;
        scoreText.fontStyle = FontStyles.Bold;
        score = 0;
        scoreText.text = score.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("score"))
        {
            score++;
            scoreText.text = score.ToString();
        }
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
