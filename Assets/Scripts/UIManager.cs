﻿using System.Collections;
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
    [SerializeField] private Text _gameOverText = null;
    private bool _isGameOver = false;
    private WaitForSeconds _gameOverFlickerDelay = new WaitForSeconds(0.25f);

    private void Start()
    {
        _scoreText.text = "Score: 0";
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
        _gameOverText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isGameOver)
        {
            bool rPressed = Input.GetKeyDown(KeyCode.R);
            if (rPressed)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
        }
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