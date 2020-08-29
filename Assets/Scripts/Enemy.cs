using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _speed = 4.0f;
    private readonly float _boundsUpper = 6.2f;
    private readonly float _boundsLower = -6.4f;

    private void Start()
    {
        float newX = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(newX, _boundsUpper, 0);
    }

    void Update()
    {

        //if (null == _player || null == _player.gameObject)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < _boundsLower)
        {
            float newX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(newX, _boundsUpper, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit: " + other.transform.name);
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.transform.GetComponent<Player>();
            if (null != player)
            {
                player.Damage();
            }
            Destroy(gameObject);

        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
