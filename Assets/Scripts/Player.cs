using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _lifes = 3;
    [SerializeField] private Laser _prefabLaser = null;
    [SerializeField] private GameObject _prefabTripleShot = null;
    private readonly float _speed = 3.5f;
    private readonly float _fireRate = 0.25f;
    private float _timestampLastShot = 0.0f;
    [SerializeReference] private bool _isTripleShotActive = false;

    void Update()
    {
        UpdateMovement();
        SpawnLaser();
    }

    private void UpdateMovement()
    {
        float boundsUpper = 0;
        float boundsLower = -4.5f;
        float boundsRight = 11.3f;
        float boundsLeft = -boundsRight;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float deltaX = inputX * Time.deltaTime * _speed;
        float deltaY = inputY * Time.deltaTime * _speed;

        Vector3 delta = new Vector3(deltaX, deltaY, 0);

        transform.Translate(delta);

        float positionX = transform.position.x;
        float positionY = transform.position.y;

        transform.position = new Vector3(positionX, Mathf.Clamp(positionY, boundsLower, boundsUpper), 0);
        //transform.position = new Vector3(positionX, Mathf.Max(Mathf.Min(positionY, boundsUpper), boundsLower), 0);

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
        _lifes -= 1;
        if (_lifes == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void SpawnLaser()
    {
        bool inputSpacePressed = Input.GetKeyDown(KeyCode.Space);
        if (inputSpacePressed && _timestampLastShot + _fireRate < Time.time)
        {
            if (_isTripleShotActive)
            {
                _ = Instantiate(_prefabTripleShot,
                                transform.position,
                                Quaternion.identity);
            }
            else
            {
                _ = Instantiate(_prefabLaser,
                                transform.position + new Vector3(0, 1.05f, 0),
                                Quaternion.identity);
            }
            _timestampLastShot = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            _isTripleShotActive = true;
            Destroy(collision.gameObject);
            StartCoroutine(DeactivatePowerUp());
        }
    }

    private IEnumerator DeactivatePowerUp()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
}
