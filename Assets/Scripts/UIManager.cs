using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText = default;
    [SerializeField] Text highscoreText = default;
    [SerializeField] Sprite[] lifeIndicatorSprites = default;
    [SerializeField] Image lifeIndicatorPlayer1 = default;
    [SerializeField] Image liveIndicatorPlayer2 = default;
    [SerializeField] Text gameOverText = default;

    static int score = 0;

    int highscore = default;
    bool isGameOver = false;
    WaitForSeconds gameOverFlickerDelay = new WaitForSeconds(0.25f);
    string highscoreKey = "highscore";

    void Start()
    {
        highscore = PlayerPrefs.GetInt(highscoreKey);
        scoreText.text = "Score: 0";
        highscoreText.text = "Highscore: " + highscore;
        gameOverText.gameObject.SetActive(false);
        if (!GameManager.IsMultiplayerSet)
        {
            liveIndicatorPlayer2.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.isOnePlayerDead = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        scoreText.text = "Score: " + score;

        if (score > highscore)
        {
            highscore = score;
            highscoreText.text = "Highscore: " + highscore;
            PlayerPrefs.SetInt(highscoreKey, highscore);
        }
    }

    public void IncreaseScore()
    {
        score += 10;        
    }

    public void UpdateLives(int playerId, int lives)
    {
        if (lives < 0 || lives > 3)
        {
            Debug.LogError("lives out of bounds");
        }
        else
        {
            if (playerId == 1)
            {
                lifeIndicatorPlayer1.sprite = lifeIndicatorSprites[lives];
            }
            else
            {
                liveIndicatorPlayer2.sprite = lifeIndicatorSprites[lives];
            }

            if (0 == lives)
            {
                isGameOver = true;
                StartCoroutine(ShowGameOver());
            }
        }
    }

    IEnumerator ShowGameOver()
    {
        while (true)
        {
            gameOverText.gameObject.SetActive(!gameOverText.gameObject.activeSelf);
            yield return gameOverFlickerDelay;
        }
    }
}
