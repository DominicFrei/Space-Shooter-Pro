using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private Text _highscoreText = null;
    [SerializeField, HideInInspector] private int _highscore = 0;
    [SerializeField] private Sprite[] _sprites = null;
    [SerializeField] private Image _lifes1 = null;
    [SerializeField] private Image _lifes2 = null;
    [SerializeField] private Text _gameOverText = null;
    private bool _isGameOver = false;
    private WaitForSeconds _gameOverFlickerDelay = new WaitForSeconds(0.25f);

    private void Start()
    {
        _scoreText.text = "Score: 0";
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
        _gameOverText.gameObject.SetActive(false);
        if (!GameManager.IsMultiplayerSet)
        {
            _lifes2.gameObject.SetActive(false);
        }
    }

    private void Update()
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
    }

    public void UpdateScore(int score, int highscore)
    {
        _scoreText.text = "Score: " + score;
        _highscoreText.text = "Highscore: " + highscore;
        _highscore = highscore;
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

    private IEnumerator ShowGameOver()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return _gameOverFlickerDelay;
        }
    }
}
