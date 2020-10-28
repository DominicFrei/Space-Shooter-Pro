using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    readonly float _rotateSpeed = 3.0f;
    [SerializeField] GameObject _explosion = null;
    SpawnManager _spawnManager = null;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            _ = Instantiate(_explosion, transform.position, Quaternion.identity);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, 3.0f);
            _ = StartCoroutine(StartSpawning());
        }
    }

    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(2.5f);
        if (null != _spawnManager)
        {
            _spawnManager.StartSpawning();
        }
    }
}
