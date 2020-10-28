using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3.0f;
    readonly float _boundsUpper = 6.2f;
    readonly float _boundsLower = -6.4f;


    void Start()
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
