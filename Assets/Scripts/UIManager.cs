using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText = default;
    [SerializeField] Text highscoreText = default;
    [SerializeField] Sprite[] _sprites = default;
    [SerializeField] Image _lifes1 = default;
    [SerializeField] Image _lifes2 = default;
    [SerializeField] Text _gameOverText = default;

    int score = 0;
    int highscore = default;
    bool _isGameOver = false;
    WaitForSeconds _gameOverFlickerDelay = new WaitForSeconds(0.25f);
    string highscoreKey = "highscore";

    void Start()
    {
        highscore = PlayerPrefs.GetInt(highscoreKey);
        scoreText.text = "Score: 0";
        highscoreText.text = "Highscore: " + highscore;
        _gameOverText.gameObject.SetActive(false);
        if (!GameManager.IsMultiplayerSet)
        {
            _lifes2.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (_isGameOver)
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
                _lifes1.sprite = _sprites[lives];
            }
            else
            {
                _lifes2.sprite = _sprites[lives];
            }

            if (0 == lives)
            {
                _isGameOver = true;
                StartCoroutine(ShowGameOver());
            }
        }
    }

    IEnumerator ShowGameOver()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return _gameOverFlickerDelay;
        }
    }
}
