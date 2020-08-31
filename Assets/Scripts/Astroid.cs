using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    private readonly float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosion = null;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            _ = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
        }
    }
}
