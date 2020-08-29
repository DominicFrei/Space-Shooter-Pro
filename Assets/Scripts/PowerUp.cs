using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField] private float _speed = 3.0f;
    private readonly float _boundsUpper = 6.2f;
    private readonly float _boundsLower = -6.4f;

    private void Start()
    {
        float newX = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(newX, _boundsUpper, 0);
    }
    
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if (transform.position.y < _boundsLower)
        {
            Destroy(gameObject);
        }
    }
}
