using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int _lifes = 3;
    [SerializeField] Laser laser = null;
    [SerializeField] bool _isTripleShotActive = false;
    [SerializeField] float _speedBoost = 1.0f;
    [SerializeField] bool _isShieldActive = false;
    [SerializeField] GameObject _shield = null;
    [SerializeField] UIManager _uiManager = null;
    [SerializeField] GameObject _rightEngine = null;
    [SerializeField] GameObject _leftEngine = null;
    [SerializeField] AudioClip _laserSoundClip = null;
    [SerializeField] AudioClip _powerUpSoundClip = null;
    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] int playerId = default;

    int _score = 0;
    int _highscore = 0;
    float _timestampLastShot = 0.0f;

    readonly float _speed = 3.5f;
    readonly float _fireRate = 0.25f;
    
    void Start()
    {
        if (!GameManager.IsMultiplayerSet)
        {
            if (1 == playerId)
            {
                transform.position = new Vector3(0.0f, -3.0f, 0.0f);
            }
            else if (2 == playerId)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (GameManager.isOnePlayerDead)
        {
            Destroy(gameObject);
        }
        else
        {
            UpdateMovement();
            SpawnLaser();
        }
    }

    void UpdateMovement()
    {
        float boundsUpper = 0;
        float boundsLower = -4.5f;
        float boundsRight = 11.3f;
        float boundsLeft = -boundsRight;

        if (1 == playerId)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * _speed * _speedBoost * Time.deltaTime);
            }
        }

        if (2 == playerId)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * _speed * _speedBoost * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * _speed * _speedBoost * Time.deltaTime);
            }
        }

        float positionX = transform.position.x;
        float positionY = transform.position.y;

        transform.position = new Vector3(positionX, Mathf.Clamp(positionY, boundsLower, boundsUpper), 0);

        if (positionX > boundsRight)
        {
            transform.position = new Vector3(boundsLeft, positionY, 0);
        }
        else if (positionX < boundsLeft)
        {
            transform.position = new Vector3(boundsRight, positionY, 0);
        }
    }

    public bool IsAlive()
    {
        return _lifes > 0;
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
        }
        else
        {
            _lifes -= 1;
            _uiManager.UpdateLives(playerId, _lifes);
        }
        switch (_lifes)
        {
            case 3:
                _rightEngine.SetActive(false);
                _leftEngine.SetActive(false);
                break;
            case 2:
                _rightEngine.SetActive(true);
                _leftEngine.SetActive(false);
                break;
            case 1:
                _rightEngine.SetActive(true);
                _leftEngine.SetActive(true);
                break;
            case 0:
                GameManager.isOnePlayerDead = true;
                Destroy(gameObject);
                break;
        }
    }

    void SpawnLaser()
    {
        bool inputSpacePressed = playerId == 1 ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.Return);
        if (inputSpacePressed && _timestampLastShot + _fireRate < Time.time)
        {
            if (_isTripleShotActive)
            {
                _ = Laser.Init(laser, transform.position + new Vector3(0, 0.9f, 0), default, Vector3.up, true);
                _ = Laser.Init(laser, transform.position + new Vector3(-0.78f, -0.5f, 0), default, Vector3.up, true);
                _ = Laser.Init(laser, transform.position + new Vector3(0.78f, -0.5f, 0), default, Vector3.up, true);
            }
            else
            {
                _ = Laser.Init(laser, transform.position + new Vector3(0, 1.05f, 0), default, Vector3.up, true);
            }
            _timestampLastShot = Time.time;

            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TripleShotPowerUp"))
        {
            _isTripleShotActive = true;
            _audioSource.clip = _powerUpSoundClip;
            _audioSource.Play();
            Destroy(collision.gameObject);
            StartCoroutine(DeactivateTripleShotPowerUp());
        }
        else if (collision.CompareTag("SpeedPowerUp"))
        {
            _speedBoost = 2.0f;
            _audioSource.clip = _powerUpSoundClip;
            _audioSource.Play();
            Destroy(collision.gameObject);
            StartCoroutine(DeactivateSpeedPowerUp());
        }
        else if (collision.CompareTag("ShieldPowerUp"))
        {
            _isShieldActive = true;
            _audioSource.clip = _powerUpSoundClip;
            _audioSource.Play();
            Destroy(collision.gameObject);
            _shield.SetActive(true);
        }
        else if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.gameObject.transform.GetComponent<Laser>();
            if (!laser.ShotByPlayer)
            {
                Damage();
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator DeactivateTripleShotPowerUp()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator DeactivateSpeedPowerUp()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoost = 1.0f;
    }

    public void EnemyKilled()
    {
        _score += 10;
        _highscore = Math.Max(_highscore, _score);
        PlayerPrefs.SetInt("highscore", _highscore);

        UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        uiManager.UpdateScore(_score, _highscore);
    }

}
