using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private Text _highscoreText = null;
    [SerializeField, HideInInspector] private int _highscore = 0;
    [SerializeField] private Sprite[] _sprites = null;
    [SerializeField] private Image _lifes = null;

    private void Start()
    {
        _scoreText.text = "Score: 0";
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
    }

    public void UpdateScore(int score, int highscore)
    {
        _scoreText.text = "Score: " + score;
        _highscoreText.text = "Highscore: " + highscore;
        _highscore = highscore;
    }

    public void UpdateLives(int lives)
    {
        if (lives < 0 || lives > 3)
        {
            Debug.LogError("lives out of bounds");
        }
        else
        {
            _lifes.sprite = _sprites[lives];
        }
    }
}
