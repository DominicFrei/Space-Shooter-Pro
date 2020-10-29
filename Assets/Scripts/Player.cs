using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerId = default;

    [SerializeField] UIManager uiManager = default;
        
    [SerializeField] GameObject rightEngine = default;
    [SerializeField] GameObject leftEngine = default;
    [SerializeField] Laser laser = default;
    [SerializeField] GameObject shield = default;

    [SerializeField] AudioSource audioSource = default;
    [SerializeField] AudioClip laserSoundClip = default;
    [SerializeField] AudioClip powerUpSoundClip = default;

    int _lifes = 3;
    readonly float _speed = 3.5f;
    readonly float _fireRate = 0.25f;
    float _speedBoost = 1.0f; 
    float _timestampLastShot = 0.0f;    
    bool _isTripleShotActive = false;
    bool _isShieldActive = false;    

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
            shield.SetActive(false);
        }
        else
        {
            _lifes -= 1;
            uiManager.UpdateLives(playerId, _lifes);
        }
        switch (_lifes)
        {
            case 3:
                rightEngine.SetActive(false);
                leftEngine.SetActive(false);
                break;
            case 2:
                rightEngine.SetActive(true);
                leftEngine.SetActive(false);
                break;
            case 1:
                rightEngine.SetActive(true);
                leftEngine.SetActive(true);
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

            audioSource.clip = laserSoundClip;
            audioSource.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TripleShotPowerUp"))
        {
            _isTripleShotActive = true;
            audioSource.clip = powerUpSoundClip;
            audioSource.Play();
            Destroy(collision.gameObject);
            StartCoroutine(DeactivateTripleShotPowerUp());
        }
        else if (collision.CompareTag("SpeedPowerUp"))
        {
            _speedBoost = 2.0f;
            audioSource.clip = powerUpSoundClip;
            audioSource.Play();
            Destroy(collision.gameObject);
            StartCoroutine(DeactivateSpeedPowerUp());
        }
        else if (collision.CompareTag("ShieldPowerUp"))
        {
            _isShieldActive = true;
            audioSource.clip = powerUpSoundClip;
            audioSource.Play();
            Destroy(collision.gameObject);
            shield.SetActive(true);
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
        uiManager.IncreaseScore();
    }

}
