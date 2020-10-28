using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player = default;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private Animator _animator = default;
    [SerializeField] private AudioSource _explosionAudio = default;
    [SerializeField] private AudioClip _explosionAudioClip = default;
    [SerializeField] private Laser laser = default;

    private bool isAlive = true;
    private readonly float _boundsUpper = 6.2f;
    private readonly float _boundsLower = -6.4f;

    private void Start()
    {
        float newX = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(newX, _boundsUpper, 0);
        _animator = GetComponent<Animator>();
        StartCoroutine(SpawnLaser());
    }

    private void Update()
    {

        if (null == _player || null == _player.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < _boundsLower)
        {
            float newX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(newX, _boundsUpper, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlive && other.CompareTag("Player"))
        {
            Player player = other.gameObject.transform.GetComponent<Player>();
            if (null != player)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            if (null != _explosionAudio)
            {
                _explosionAudio.clip = _explosionAudioClip;
                _explosionAudio.Play();
            }
            isAlive = false;
            Destroy(gameObject, 2.8f);

        }
        else if (isAlive && other.CompareTag("Laser"))
        {
            Laser laser = other.gameObject.transform.GetComponent<Laser>();
            if (laser.ShotByPlayer)
            {
                if (null != _player)
                {
                    _player.EnemyKilled();
                }
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 0.0f;
                if (null != _explosionAudio)
                {
                    _explosionAudio.clip = _explosionAudioClip;
                    _explosionAudio.Play();
                }
                Destroy(GetComponent<Collider2D>());
                Destroy(other.gameObject);
                isAlive = false;
                Destroy(gameObject, 2.8f);
            }
        }
    }

    IEnumerator SpawnLaser()
    {
        while (true)
        {
            WaitForSeconds laserSpawnRate = new WaitForSeconds(Random.Range(2.0f, 5.0f));
            yield return laserSpawnRate;
            if (isAlive)
            {
                _ = Laser.Init(laser, transform.position + new Vector3(0.0f, -0.5f, 0), Quaternion.identity, Vector3.down, false);
            }
        }

    }

}
